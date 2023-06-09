using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DonutDiner.ItemModule.Items
{
    public class Donut : MonoBehaviour, IItem
    {
        #region Fields

        [SerializeField] private ItemObject _item;
        [SerializeField] private int _quantity = 1;

        #endregion

        #region Properties

        public ItemObject Root => _item;

        #endregion

        #region Public Methods

        public void AddToInventory()
        {
            // ADD TO INVENTORY

            gameObject.SetActive(false);
        }

        #endregion
    }
}