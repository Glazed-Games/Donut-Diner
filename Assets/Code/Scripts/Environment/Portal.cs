using UnityEngine;

namespace DonutDiner.EnvironmentModule
{
    public class Portal : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _targetPortal;
        [SerializeField] private Camera _portalCamera;
        [SerializeField] private bool _isEnabled;
        [SerializeField] private LayerMask _layersToTeleport;

        private Camera _mainCamera;
        private Collider _collider;
        private MeshRenderer _renderer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void OnEnable()
        {
            _collider.enabled = _isEnabled;

            if (_isEnabled) SetCameraTexture();
        }

        public void LateUpdate()
        {
            if (!_isEnabled) return;

            Quaternion targetRotation = Quaternion.LookRotation(-_targetPortal.forward, _targetPortal.up);
            Quaternion cameraRotation = Quaternion.Inverse(transform.rotation) * targetRotation;
            Vector3 offset = _mainCamera.transform.position - transform.position;

            _portalCamera.transform.SetPositionAndRotation(_targetPortal.position + cameraRotation * offset, cameraRotation * _mainCamera.transform.rotation);
            _portalCamera.projectionMatrix = _mainCamera.projectionMatrix;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!ShouldTeleport(other)) return;

            if (IsWrongSide(other) || IsFacingOppositeDirection(other)) return;

            if (other.TryGetComponent(out CharacterController controller)) controller.enabled = false;

            Teleport(other.transform);

            if (controller) controller.enabled = true;

            _isEnabled = false;
            _collider.enabled = false;

            _targetPortal.GetComponent<Portal>().ActivatePortal();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!ShouldTeleport(other)) return;

            if (!IsWrongSide(other) || !IsFacingOppositeDirection(other)) return;

            if (other.TryGetComponent(out CharacterController controller)) controller.enabled = false;

            Teleport(other.transform);

            if (controller) controller.enabled = true;

            _isEnabled = false;
            _collider.enabled = false;

            _targetPortal.GetComponent<Portal>().ActivatePortal();
        }

        #endregion

        #region Public Methods

        public void ActivatePortal()
        {
            SetCameraTexture();

            _isEnabled = true;
            _collider.enabled = true;
        }

        #endregion

        #region Private Methods

        private void Teleport(Transform other)
        {
            Quaternion currentRotation = Quaternion.LookRotation(-transform.forward, transform.up);
            Quaternion targetRotation = Quaternion.Inverse(currentRotation) * _targetPortal.rotation;
            Vector3 offset = other.position - transform.position;

            other.SetPositionAndRotation(_targetPortal.position + targetRotation * offset, targetRotation * other.rotation);
        }

        private void SetCameraTexture()
        {
            if (_portalCamera.targetTexture) _portalCamera.targetTexture.Release();

            _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            _renderer.material.mainTexture = _portalCamera.targetTexture;
        }

        private bool ShouldTeleport(Collider other)
        {
            return _layersToTeleport == (_layersToTeleport | (1 << other.gameObject.layer));
        }

        private bool IsWrongSide(Collider other)
        {
            return Vector3.Dot(transform.forward, other.transform.position - transform.position) > 0.0f;
        }

        private bool IsFacingOppositeDirection(Collider other)
        {
            return Vector3.Dot(transform.forward, other.transform.forward) < 0.0f;
        }

        private void SetReferences()
        {
            _mainCamera = Camera.main;
            _collider = GetComponent<Collider>();
            _renderer = GetComponent<MeshRenderer>();
        }

        #endregion
    }
}