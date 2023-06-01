using DonutDiner.ItemModule;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DonutDiner.UIModule.Menu
{
    public class UIInventoryButton : MonoBehaviour
    {
        [SerializeField] private int numberInList;
        [SerializeField] private UIInventory uIInventory;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;


        public void OnClick() 
        {
            if (uIInventory != null)
            {
                uIInventory.ItemButtonOnClick(numberInList);
            }
        }
        public void SetUIInventory(UIInventory inventory,int _numberInList)
        {
            uIInventory = inventory;
            numberInList = _numberInList;
        }

        public void SetButton(ItemObject item)
        {
            if (itemImage != null)
            {
                itemImage.sprite = item.Icon;
            }

            if (itemName != null)
            {
                itemName.text = item.Name;
                //NOTE: adding the number to the end for testing and display purposes
                itemName.text += numberInList.ToString();
            }
        }
        public void SetButton(string displayName)
        {
       
            if (itemName != null)
            {
                itemName.text = displayName;
            }
        }
        public Image GetImage()
        { return itemImage; }

        public int NumberInList()
        { return numberInList; }

    }

    

}
