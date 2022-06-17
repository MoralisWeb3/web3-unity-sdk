using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Abstractions;
using MoralisUnity.Platform.Operations;
using MoralisUnity.Platform.Utilities;

namespace MoralisUnity.Platform.Objects
{
    /// <summary>
    /// Base class for any objects that can be processed via Moralis. 
    /// Note: JsonIgnore internals just incase someone changes JsonSerializerSettings 
    /// </summary>
    public class MoralisObject
    {
        private IObjectService objectService = null;

        public MoralisObject()
        {
            this.ClassName = this.GetType().Name;
            this.objectId = null; 
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
            this.ACL = new MoralisAcl();
            this.IsDirty = false;
            this.sessionToken = string.Empty;
        }

        public MoralisObject(string className, 
            string objectId = null,
            string sessionToken = null,
            DateTime? createdAt = null,
            DateTime? updatedAt = null,
            MoralisAcl ACL = null)
        {
            this.ClassName = className;
            this.objectId = objectId;
            this.sessionToken = sessionToken;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
            this.ACL = ACL;
            this.IsDirty = true;
        }

        public string objectId;
        public string sessionToken;
        public DateTime? createdAt;
        public DateTime? updatedAt;
        public MoralisAcl ACL; 
        public string ClassName { get; set; }
        internal bool IsDirty { get; set; }
        internal IObjectService ObjectService 
        {
            get
            {
                if (objectService == null)
                {
                    // This functionality is only available in a Unity Context and
                    // not in the SDK outside of Unity.
                    if (MoralisState.Initialized.Equals(Moralis.State))
                    { 
                        // Initialize Object Service from the Moralis Client.
                        objectService = Moralis.Client?.ServiceHub?.ObjectService;
                    }
                }

                return objectService;
            }

            set
            {
                objectService = value;
            }
        }

        internal object Mutex { get; } = new object { };
        internal IDictionary<string, IMoralisFieldOperation> CurrentOperations
        {
            get
            {
                lock (Mutex)
                {
                    return OperationSetQueue.Count > 0 ? OperationSetQueue.Last.Value : new Dictionary<string, IMoralisFieldOperation>();
                }
            }
        }

        LinkedList<IDictionary<string, IMoralisFieldOperation>> OperationSetQueue { get; } = new LinkedList<IDictionary<string, IMoralisFieldOperation>>();

        /// <summary>
        /// Deletes this object on the server.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public UniTask DeleteAsync(CancellationToken cancellationToken = default)
        {
            return this.ObjectService.DeleteAsync(this, sessionToken, cancellationToken);
        }

        public virtual async UniTask<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            // MoralisUser is a special case not all properties can be passed to save.
            if (this is MoralisUser) ((MoralisUser)this).SetSaving(true);

            IDictionary<string, IMoralisFieldOperation> operations = this.StartSave();
            string resp = await this.ObjectService.SaveAsync(this, operations, sessionToken, cancellationToken);
                
            Dictionary<string, object> obj = JsonUtilities.Parse(resp) as Dictionary<string, object>;

            if (obj.ContainsKey("objectId"))
            {
                this.objectId = (string)obj["objectId"];
            }

            if (obj.ContainsKey("createdAt"))
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(obj["createdAt"].ToString(), out dt))
                {
                    this.createdAt = dt;
                }
            }

            if (obj.ContainsKey("updatedAt"))
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(obj["updatedAt"].ToString(), out dt))
                {
                    this.updatedAt = dt;
                }
            }

            // Set user saving false so that local persistence has full object.
            if (this is MoralisUser) ((MoralisUser)this).SetSaving(false);

            Console.WriteLine($"Save response: {resp}");

            OperationSetQueue.Clear();

            return true;
        }

        /// <summary>
        /// Atomically increments the given key by 1.
        /// </summary>
        /// <param name="key">The key to increment.</param>
        public void Increment(string key) => Increment(key, 1);

        /// <summary>
        /// Atomically increments the given key by the given number.
        /// </summary>
        /// <param name="key">The key to increment.</param>
        /// <param name="amount">The amount to increment by.</param>
        public void Increment(string key, byte amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        public void Increment(string key, int amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        public void Increment(string key, long amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        /// <summary>
        /// Atomically increments the given key by the given number.
        /// </summary>
        /// <param name="key">The key to increment.</param>
        /// <param name="amount">The amount to increment by.</param>
        public void Increment(string key, double amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        public void Increment(string key, decimal amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        public void Increment(string key, float amount)
        {
            lock (Mutex)
            {
                PerformOperation(key, new MoralisIncrementOperation(amount));
            }
        }
        /// <summary>
        /// Pushes new operations onto the queue and returns the current set of operations.
        /// </summary>
        internal IDictionary<string, IMoralisFieldOperation> StartSave()
        {
            lock (Mutex)
            {
                IDictionary<string, IMoralisFieldOperation> currentOperations = CurrentOperations;
                OperationSetQueue.AddLast(new Dictionary<string, IMoralisFieldOperation>());
                return currentOperations;
            }
        }

        internal string GetCurrentSessionToken() => this.sessionToken;

        /// <summary>
        /// PerformOperation is like setting a value at an index, but instead of
        /// just taking a new value, it takes a MoralisFieldOperation that modifies the value.
        /// </summary>
        internal void PerformOperation(string key, IMoralisFieldOperation operation)
        {
            lock (Mutex)
            {
                
                PropertyInfo prop = this.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                
                if (null != prop && prop.CanWrite)
                {
                    object oldValue = prop.GetValue(this);
                    object newValue = operation.Apply(oldValue, key);
                    prop.SetValue(this, newValue);
                }

                bool wasDirty = CurrentOperations.Count > 0;
                CurrentOperations.TryGetValue(key, out IMoralisFieldOperation oldOperation);
                IMoralisFieldOperation newOperation = operation.MergeWithPrevious(oldOperation);
                CurrentOperations[key] = newOperation;

            }
        }
    }
}
