using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public class BlackLightPiece : MonoBehaviour
    {
        public Material reveal;
        public Light light;

        void Start()
        {

        }

        void Update()
        {
            if (reveal && light)
            {
                reveal.SetVector("_LightPosition", light.transform.position);
                reveal.SetVector("_LightDirection", -light.transform.forward);
                reveal.SetFloat("_LightAngle", light.spotAngle);
            }
        }// Start is called before the first frame update
        
    }
}