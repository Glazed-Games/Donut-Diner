using DonutDiner.PlayerModule;
using UnityEngine;

namespace DonutDiner.ItemModule
{
    [CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Items/New Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        #region Fields

        [SerializeField] private ItemObject[] _allItems;

        #endregion

        #region Properties

        public ItemObject[] AllItems => _allItems;

        #endregion

        #region Public Methods

        public void InstantiateItem(string itemId)
        {
            foreach (ItemObject item in _allItems)
            {
                if (item.Id == itemId)
                {
                    Instantiate(item.Prefab, Player.Transform.position, Quaternion.identity);
                }
            }
        }

        #endregion
    }
}