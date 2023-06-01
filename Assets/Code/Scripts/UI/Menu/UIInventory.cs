using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DonutDiner.FrameworkModule;
using DonutDiner.ItemModule;

namespace DonutDiner.UIModule.Menu
{
    public class UIInventory : MonoBehaviour
    {
        //  [SerializeField] private List<UIItemRow> _itemRows;

        //public string Name => _name;
        //public string Id => _uniqueId;
        //public string Description => _description;
        //public Sprite Icon => _icon;
        //public GameObject Prefab => _prefab;

        [SerializeField] private RectTransform panel_page;
        [SerializeField] private List<UIInventoryButton> buttonList;
        [SerializeField] private UIInventoryButton prefab_button;
        [SerializeField] private Transform parent_buttons;

        [SerializeField] private Image image_selectedItem;
        [SerializeField] private TMP_Text text_selectedItem;
        [SerializeField] private TMP_Text text_selectedItem_Description;
        

        private int page; //the portion of the inventory currently displayed
        [Min(1)] private int ROWS = 3;
        [Min(1)] private int COLUMNS = 3;

        private int pageCount;
        private int TEST_PAGECOUNT = 5;

        public void Start()
        {
            CreateButtons();
        }

        public void CreateButtons()
        {
            if (prefab_button == null) { return; }

            int rowCount = 0;
            int columnCount = 0;
           
            float width = panel_page.rect.width / (COLUMNS );
            float height = panel_page.rect.height / (ROWS );

            buttonList = new List<UIInventoryButton>();

            while (rowCount < ROWS)
            {
                while (columnCount < COLUMNS)
                {
                    UIInventoryButton clone = Instantiate(prefab_button, parent_buttons.position, parent_buttons.rotation) ;
                    clone.transform.parent = parent_buttons;
                    clone.transform.localPosition = new Vector2((width * (columnCount - 1)) ,(height * (rowCount - 1)));
                        
                    buttonList.Add(clone.GetComponent<UIInventoryButton>());
                    clone.SetUIInventory(this, buttonList.Count);
                     
                    columnCount++;
                }
                columnCount = 0;
                rowCount++;
            }
        }

        public void ItemButtonOnClick(int buttonNumber)
        {
            Debug.Log(buttonNumber);
            int itemNumber = (page * (ROWS * COLUMNS)) + buttonNumber;
            List<ItemObject> inventory = PlayerInventory.Instance.GetInventory();
            if (inventory.Count > 0 && inventory.Count > itemNumber)
            {
                ItemObject item = inventory[itemNumber];
                if (image_selectedItem) { image_selectedItem.sprite = item.Icon; }
                if (text_selectedItem) { text_selectedItem.text = item.Name; }
                if (text_selectedItem_Description) { text_selectedItem_Description.text = item.Description; }

            }
            else
            {
                //NOTE:for debug testing without an item list
                if (image_selectedItem) { image_selectedItem.sprite = ButtonList()[buttonNumber - 1].GetImage().sprite; }
                if (text_selectedItem) 
                { 
                    text_selectedItem.text = "Donut #" + itemNumber.ToString();
                }
                if (text_selectedItem_Description) { text_selectedItem_Description.text = "One of your -" + PlayerInventory.Instance.GetInventory().Count + "- donuts"; }
            }

        }

        public void ChangePage(int direction)
        {
            page += direction;
            List<ItemObject> inventory = PlayerInventory.Instance.GetInventory();

            pageCount = inventory.Count / (ROWS * COLUMNS);

            if (pageCount == 0)
            {
                pageCount = TEST_PAGECOUNT;
            }
            if (page >= pageCount)
            {
                //wrap the page around to the end of the list
                page = 0;
            }
            else if (page < 0 && ROWS * COLUMNS != 0)
            {
                //wrap the page around to the end of the list
                page = inventory.Count / (ROWS * COLUMNS);
                if (page == 0)
                {
                    page = TEST_PAGECOUNT;
                }
                
                page -= 1;
            }
           // UpdateItemButtons(inventory.GetRange(page * (ROWS * COLUMNS), (ROWS * COLUMNS)));
            UpdateItemButtons();
        }

        public void UpdateItemButtons(List<ItemObject> items)
        {

            foreach (UIInventoryButton btn in ButtonList())
            {
                if (btn.NumberInList() >= 0 && btn.NumberInList() < items.Count)
                {
                    btn.SetButton(items[btn.NumberInList()]);

                }
            }
        }

        public void UpdateItemButtons()
        {

            foreach (UIInventoryButton btn in ButtonList())
            {
                 btn.SetButton("Donut: #" + ((page * pageCount) + btn.NumberInList())); 
            }
        }

        public List<UIInventoryButton> ButtonList()
        {
            if (buttonList == null || buttonList.Count == 0)
            {
                CreateButtons();
            }

            return buttonList;
                
        }


    }
}