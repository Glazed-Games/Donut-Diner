using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DonutDiner.UIModule.Menu
{
    public class UIDialogue : MonoBehaviour
    {

        [SerializeField] private List<UIDialogueOptionButton> responseButtons;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<UIDialogueOptionButton> ResponseButtons()
        {
            if (responseButtons == null) { responseButtons = new List<UIDialogueOptionButton>(); }
   

            return responseButtons;
        }

    }
}