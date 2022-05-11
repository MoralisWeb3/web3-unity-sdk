using UnityEngine;

namespace MoralisUnity.Sdk.Components
{
    /// <summary>
    /// Some workflows allow too many components too
    /// easily (e.g. Camera, EventSystem).
    ///
    /// This is generally harmless but Unity throws warnings.
    ///
    /// So this component will remove self if needed.
    /// </summary>
    public class EnsureUniqueInScene : MonoBehaviour
    {
        //  Fields  ---------------------------------------
        [SerializeField] 
        private Component _component = null;
        
        //  Unity Methods  --------------------------------
        protected void Awake()
        {
            var x = GameObject.FindObjectsOfType(_component.GetType());
            if (x.Length > 1)
            {
                GameObject.Destroy(_component.gameObject);
            }
        }
    }
}