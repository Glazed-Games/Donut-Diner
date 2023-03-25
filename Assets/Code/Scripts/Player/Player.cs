using UnityEngine;

namespace DonutDiner.PlayerModule
{
    public class Player : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Camera _firstPersonCamera;
        [SerializeField] private Transform _playerSight;
        [SerializeField] private Transform _playerHand;
        [SerializeField] private Transform _itemPlacement;
        [SerializeField] private GameObject _lightSource;

        #endregion

        #region Properties

        public static Transform Transform { get; private set; }
        public static Transform Sight { get; private set; }
        public static Transform Hand { get; private set; }
        public static Vector3 SightPosition { get; private set; }
        public static Camera FirstPersonCamera { get; private set; }
        public static Transform InspectionItemPlacement { get; private set; }
        public static GameObject InspectionLightSource { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Transform = transform;
            Sight = _playerSight;
            Hand = _playerHand;
            SightPosition = _playerSight.localPosition;
            FirstPersonCamera = _firstPersonCamera;
            InspectionItemPlacement = _itemPlacement;
            InspectionLightSource = _lightSource;
        }

        #endregion
    }
}