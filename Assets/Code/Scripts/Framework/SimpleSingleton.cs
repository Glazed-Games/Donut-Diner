using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public abstract class SimpleSingleton<T> : MonoBehaviour where T : SimpleSingleton<T>
    {
        #region Properties

        public static T Instance { get; private set; }

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                DestroyImmediate(this);

                return;
            }
        }

        #endregion
    }
}