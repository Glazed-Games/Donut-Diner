using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DonutDiner.FrameworkModule;
namespace DonutDiner.UIModule.Menu
{
    public class UIDialogueOptionButton : MonoBehaviour
    {
        [SerializeField] private int numberInList;
        [SerializeField] private TMP_Text response;

        public void OnClick()
        {
            DialogueManager.Instance.DialogueResponse(numberInList);
        }

        public void SetUIInventory(UIInventory inventory, int _numberInList)
        {
            numberInList = _numberInList;
        }



        public void SetButton(string responseText)
        {

           
        }
        public void SetButton(string responseText,int listNumber)
        {
            if (response != null)
            {
                response.SetText(responseText);
            }
            numberInList = listNumber;
        }

        public int NumberInList()
        { return numberInList; }
    }
}