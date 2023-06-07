using DonutDiner.FrameworkModule;
using DonutDiner.PlayerModule;
using UnityEngine;

namespace DonutDiner.ItemModule.Items
{
    public class PlayerHeldLight : MonoBehaviour
    {
        [SerializeField] private GameObject pointLight;

        public void OnPickUp()
        {
            Player.FirstPersonCamera.cullingMask |= 1 << LayerMask.NameToLayer(Layer.Blacklight);

            if (pointLight) { pointLight.SetActive(true); }
        }

        public void OnDrop()
        {
            Player.FirstPersonCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(Layer.Blacklight));

            if (pointLight) { pointLight.SetActive(false); }
        }
    }
}