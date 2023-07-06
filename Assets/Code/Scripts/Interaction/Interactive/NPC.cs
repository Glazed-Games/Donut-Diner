using DonutDiner.NPCModule;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive
{
    public class NPC : MonoBehaviour, IInteractive
    {
        #region Fields

        [SerializeField] private CharacterObject _character;
        [SerializeField] private TextAsset _dialogue;
        [SerializeField] private bool _isLocked;

        [SerializeField] private bool _canInteract = true;

        #endregion Fields

        #region Properties

        public bool CanInteract { get => _canInteract; set => _canInteract = value; }

        #endregion Properties

        #region Public Methods

        public bool IsInteractable()
        {
            throw new System.NotImplementedException();
        }

        public void IsInteractable(bool value)
        {
            throw new System.NotImplementedException();
        }

        public bool IsLocked()
        {
            return _isLocked;
        }

        public void IsLocked(bool value)
        {
            _isLocked = !value;
        }

        public virtual void StartInteraction()
        {
            if (!_canInteract) return;


            //DialogueManager.Instance.StartDialogue(new DialogueDTO(_dialogue.text, _character.Name));
        }

        public string CharacterName()
        {
            return _character.Name;    
        }

        #endregion Public Methods
    }
}