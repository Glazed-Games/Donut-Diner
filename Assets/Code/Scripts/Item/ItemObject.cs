using UnityEditor;
using UnityEngine;

namespace DonutDiner.ItemModule
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Items/New Item")]
    public class ItemObject : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;

        [Space]
        [SerializeField] private string _uniqueId;

        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _icon;

        [Space]
        [TextArea(10, 30)]
        [SerializeField] private string _description;

        #endregion Fields

        #region Properties

        public string Name => _name;
        public string Id => _uniqueId;
        public string Description => _description;
        public Sprite Icon => _icon;
        public GameObject Prefab => _prefab;

        #endregion Properties

        #region Unity Methods

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(_uniqueId)) return;

            string path = AssetDatabase.GetAssetPath(this);

            _uniqueId = AssetDatabase.AssetPathToGUID(path);
        }

        #endregion Unity Methods
    }
}