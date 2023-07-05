using System.Collections.Generic;
using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public class DialogueManager : ComponentSingleton<DialogueManager>
    {
        #region Fields

        public float SceneTime = 0.0f;

        private string _currentScene = "null";
        private bool _reloadSaveFile = false;
        private DialogueObject activeDialogue;
        private int currentLine;

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

        #region Public Methods

        public void StartDialogue(DialogueObject dialogue)
        {
            //display text
            //tell audio to play voice line
            //start timer to show responses or display next line of the speech
            currentLine = 0;

        }

        public void AdvanceDialogue( )
        {
            if (activeDialogue == null) { return; }

            currentLine++;

            if (activeDialogue.Dialogue.Length > currentLine)
            {
                //show next line
                return;
            }

            if (activeDialogue.HasResponses)
            {
                //show response
                return;
            }

            //otherwise end dialogue
        }


        public void DialogueResponse(int responseSelected)
        {
            if (activeDialogue == null) { return; }
            if (activeDialogue.HasResponses == false) { return; }
        }
        



        #endregion Public Methods
    }










}