using DonutDiner.FrameworkModule.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DonutDiner.FrameworkModule
{
    // Singleton class for high-level management of game simulation
    //   - Source of general global data
    //   - Sole source of Scene Management

    // First implemented by Ben Russell

    public class GameManager : ComponentSingleton<GameManager>
    {
        #region Fields

        [HideInInspector] public SerializationManager SerializationManager;

        public float SceneTime = 0.0f; // Time spent in current scene thus-far

        private string _currentScene = "null";
        private bool _reloadSaveFile = false;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR

        private void Reset()
        {
            // Add subsystems on creation in editor
            SerializationManager = FindObjectOfType<SerializationManager>();

            if (!SerializationManager)
            {
                SerializationManager = new GameObject("Serialization Manager").AddComponent<SerializationManager>();
            }
        }

#endif

        private void Update()
        {
            SceneTime += Time.deltaTime;

            // Tmp keycodes for testing
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                GameData.SaveToFile();
            }
            else if (Keyboard.current.f8Key.wasPressedThisFrame)
            {
                GameData.LoadFromFile();
            }
        }

        private void FixedUpdate()
        {
            if (_reloadSaveFile)
            {
                // Reload save file now that scene is loaded
                _reloadSaveFile = false;
                GameData.LoadFromFile(-1, true);
            }
        }

        #endregion

        #region Public Methods

        public string GetCurrentSceneName() => _currentScene;

        public void LoadScene(string scene, bool reloadSaveFile = false)
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scene);

            if (sceneIndex >= 0)
            {
                // Scene found
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);

                _currentScene = scene;
                _reloadSaveFile = reloadSaveFile;
                SceneTime = 0.0f;
            }
            else
            {
                Debug.LogError("Scene: " + scene.ToString() + " not found, has it been added in the Build Settings?");
            }
        }

        #endregion

        #region Protected Methods

        protected override void Initialize()
        {
            _currentScene = SceneManager.GetActiveScene().name;
        }

        #endregion
    }
}