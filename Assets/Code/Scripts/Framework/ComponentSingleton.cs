using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public abstract class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
    {
        #region Fields

        private static T _instance;
        private static bool _isInitialized;
        private static bool _isApplicationQuitting;

        #endregion

        #region Properties

        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting) return null;

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject()
                        {
                            name = typeof(T).Name
                        };

                        _instance = gameObject.AddComponent<T>();
                    }

                    if (!_isInitialized)
                    {
                        _isInitialized = true;
                        _instance.Initialize();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                DestroyImmediate(this);

                return;
            }

            if (!_isInitialized)
            {
                DontDestroyOnLoad(gameObject);

                _instance.Initialize();
                _isInitialized = true;
            }
        }

        private void OnApplicationQuit()
        {
            _instance = null;
            _isApplicationQuitting = true;
        }

        #endregion

        #region Abstract Methods

        protected abstract void Initialize();

        #endregion
    }
}