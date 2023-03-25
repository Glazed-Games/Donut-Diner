using UnityEditor;
using UnityEngine;

namespace DonutDiner.NPCModule
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "NPCs/New Character")]
    public class CharacterObject : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _characterName;

        [Space]
        [SerializeField] private string _uniqueId;
        [SerializeField] private Sprite _sprite;

        [Space]
        [TextArea(10, 30)]
        [SerializeField] private string _description;

        #endregion

        #region Properties

        public string Name => _characterName;
        public string Id => _uniqueId;
        public string Description => _description;
        public Sprite Sprite => _sprite;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(_uniqueId)) return;

            string path = AssetDatabase.GetAssetPath(this);

            _uniqueId = AssetDatabase.AssetPathToGUID(path);
        }

        #endregion
    }
}