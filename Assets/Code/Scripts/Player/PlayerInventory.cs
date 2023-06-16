using DonutDiner.FrameworkModule;
using DonutDiner.ItemModule;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DonutDiner.PlayerModule
{
    public class PlayerInventory : ComponentSingleton<PlayerInventory>
    {
        // Using a list because the inventory will change size
        [SerializeField] private List<ItemObject> inventory;

        [SerializeField] private List<ItemObject> donuts;
        [SerializeField] private UnityEvent _useItemEvent;

        public delegate void TryUseItem(ItemObject item);

        public static TryUseItem tryUseItem;

        // Start is called before the first frame update
        private void Start()
        {
            tryUseItem += TryItem;
            foreach (ItemObject el in ItemPooler.Instance.ItemDatabase.AllItems)
            {
                GetInventory().Add(el);
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void TryItem(ItemObject item)
        {
            Debug.Log("TRY HANDLE USE ITEM");
        }

        #region Public Methods

        public void AddItem(string itemID)
        {
            //Using the database all items rather than the items to examine as to not require an object be instantiated
            // to be used or 'in' the player inventory
            foreach (ItemObject el in ItemPooler.Instance.ItemDatabase.AllItems)
            {
                if (el.Id.Equals(itemID))
                {
                    AddItemToInventory(el);
                    return;
                }
            }
        }

        public void AddItem(GameObject itemObj)
        {
            foreach (ItemObject el in ItemPooler.Instance.ItemDatabase.AllItems)
            {
                if (el.Name.Equals(itemObj.name))
                {
                    AddItemToInventory(el);
                    return;
                }
            }
        }

        public void AddItemToInventory(ItemObject newItem)
        {
            GetInventory().Add(newItem);
        }

        public ItemObject GetItem(string itemID)
        {
            foreach (ItemObject el in GetInventory())
            {
                if (el.Name.Equals(itemID))
                {
                    return el;
                }
            }

            return null;
        }

        public ItemObject GetItem(int placeInList)
        {
            if (placeInList >= 0 && GetInventory().Count > placeInList)
            {
                return GetInventory()[placeInList];
            }

            return null;
        }

        public bool CheckForItem(ItemObject item)
        {
            return GetInventory().Contains(item);
        }

        public List<ItemObject> GetInventory()
        {
            if (inventory == null)
            { inventory = new List<ItemObject>(); }

            return inventory;
        }

        public List<ItemObject> GetDonuts()
        {
            if (donuts == null)
            {
                donuts = new List<ItemObject>();
            }

            return donuts;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Initialize()
        {
        }

        #endregion Protected Methods
    }
}