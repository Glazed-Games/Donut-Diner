using DonutDiner.FrameworkModule;
using DonutDiner.PlayerModule;
using UnityEngine;

namespace DonutDiner.ItemModule.Items
{
    public class ItemToInputInto : MonoBehaviour, IItem, IPuzzlePiece
    {
        #region Fields

        [SerializeField] private ItemObject _item;
        [SerializeField] private Puzzle _puzzle;
        [SerializeField] private string _unlockCode;
        [SerializeField] private string lastCodeEntered;

        private MeshRenderer _renderer;
        private Collider _collider;
        private GameObject _prefab;

        #endregion Fields

        #region Properties

        public ItemObject Root => _item;

        #endregion Properties

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        #endregion Unity Methods

        #region Public Methods

        public GameObject GetItemPrefab()
        {
            if (!ItemPooler.Instance.ItemsToExamine.TryGetValue(_item.Id, out _prefab)) return null;

            return _prefab;
        }

        public void PrepareInspection()
        {
            _renderer.enabled = false;
            _collider.enabled = false;

            _prefab.SetActive(true);
            _prefab.layer = LayerMask.NameToLayer(Layer.FirstPersonCamera);
            _prefab.transform.position = Player.InspectionItemPlacement.position;
            _prefab.transform.LookAt(Player.FirstPersonCamera.transform);

            if (_prefab.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = true;
            }
        }

        public void FinishInspection()
        {
            _renderer.enabled = true;
            _collider.enabled = true;

            _prefab.SetActive(false);
            _prefab = null;
        }

        #endregion Public Methods

        #region Private Methods

        private void SetReferences()
        {
            _renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
        }

        public void GetTextInput(string code)
        {
            lastCodeEntered = code;
            if (_puzzle) { _puzzle.TryToSolve(); }
        }

        public void SetPuzzle(Puzzle newPuzzle)
        {
            _puzzle = newPuzzle;
        }

        public bool IsSolved()
        {
            return lastCodeEntered == _unlockCode;
        }

        public void CheckSolution()
        {
            throw new System.NotImplementedException();
        }

        #endregion Private Methods
    }
}