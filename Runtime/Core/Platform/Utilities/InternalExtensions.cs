
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace MoralisUnity.Platform.Utilities
{
    /// <summary>
    /// Provides helper methods that allow us to use terser code elsewhere.
    /// </summary>
    public static class InternalExtensions
    {
        /// <summary>
        /// Ensures a task (even null) is awaitable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static UniTask<T> Safe<T>(this UniTask<T> task)
        {
            if (task is { })
                return task;
            else
                return UniTask.FromResult(default(T));
        }

        /// <summary>
        /// Ensures a task (even null) is awaitable.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static UniTask Safe(this UniTask task)
        {

            if (task is { })
                return task;
            else
                return UniTask.FromResult<object>(null);
        }

        public delegate void PartialAccessor<T>(ref T arg);

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self,
            TKey key,
            TValue defaultValue)
        {
            if (self.TryGetValue(key, out TValue value))
                return value;
            return defaultValue;
        }

        public static bool CollectionsEqual<T>(this IEnumerable<T> a, IEnumerable<T> b) => Equals(a, b) ||
                   a != null && b != null &&
                   a.SequenceEqual(b);

        public static async void Wait<T>(this UniTask<T> task) //=> task.ContinueWith(t =>
        {
            if (UniTaskStatus.Pending.Equals(task.Status))
            {
                await task;
            }
        }

        public static async UniTask<T> Result<T>(this UniTask<T> task)
        {
            UniTask<T> result = default;

            if (UniTaskStatus.Pending.Equals(task.Status))
                await task;

            if (!task.Status.Equals(UniTaskStatus.Faulted) && !task.Status.Equals(UniTaskStatus.Canceled))
            {
                result = task;
            }

            return result.GetAwaiter().GetResult();
        }
    }
}
