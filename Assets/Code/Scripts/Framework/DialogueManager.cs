namespace DonutDiner.FrameworkModule
{
    public class DialogueManager : ComponentSingleton<DialogueManager>
    {
        #region Fields

        public float SceneTime = 0.0f;

        private string _currentScene = "null";
        private bool _reloadSaveFile = false;

        #endregion Fields

        #region Unity Methods

#if UNITY_EDITOR

        private void Reset()
        {
        }

#endif

        private void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        #endregion Unity Methods



        #region Protected Methods

        protected override void Initialize()
        {
        }

        #endregion Protected Methods
    }
}