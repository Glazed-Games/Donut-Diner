using DonutDiner.FrameworkModule;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DonutDiner.ItemModule
{
    public class ItemPooler : ComponentSingleton<ItemPooler>
    {
        #region Fields

        [SerializeField] private ItemDatabase _itemDatabase;

        #endregion

        #region Properties

        public ItemDatabase ItemDatabase { get; private set; }
        public Dictionary<string, GameObject> ItemsToExamine { get; private set; }

        #endregion

        #region Protected Methods

        protected override void Initialize()
        {
            if (_itemDatabase == null) _itemDatabase = LoadItemDatabase();

            ItemDatabase = _itemDatabase;

            CreatePool();
        }

        #endregion

        #region Private Methods

        private void CreatePool()
        {
            ItemsToExamine = new Dictionary<string, GameObject>();

            foreach (ItemObject item in _itemDatabase.AllItems)
            {
                GameObject prefab = Instantiate(item.Prefab);

                prefab.SetActive(false);
                prefab.transform.parent = transform;

                ItemsToExamine.Add(item.Id, prefab);
            }
        }

#if UNITY_EDITOR

        private static ItemDatabase LoadItemDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemDatabase");

            if (guids.Length == 0)
            {
                var instance = ScriptableObject.CreateInstance<ItemDatabase>();

                AssetDatabase.CreateAsset(instance, "Assets/Scriptable Objects/Database/Item Database.asset");

                return instance;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);

            return AssetDatabase.LoadAssetAtPath<ItemDatabase>(path);
        }

#endif

        #endregion
    }
}