using DonutDiner.FrameworkModule;
using DonutDiner.FrameworkModule.Data;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive
{
    public class LightReflector : SerializableObject, IPuzzlePiece
    {
        [SerializeField] private Puzzle puzzle;
        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;
        [SerializeField] private bool hasLight;

        [SerializeField] private Transform raycastFrom;
        [SerializeField] private GameObject beamOfLightObject;
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Material offColor;
        [SerializeField] private Material onColor;

        private float spherecastRadius = 0.1f;
        private float spherecastDistance = 20;

        [SerializeField] private LightReflector reflectedLightTarget; //the reflector that this reflector is bouncing light onto

        public void SetPuzzle(Puzzle newPuzzle)
        { puzzle = newPuzzle; }

        public Puzzle GetPuzzle()
        { return puzzle; }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void ReceiveLight(bool on)
        {
            if (HasLight() == on) { return; }
            hasLight = on;

            if (!on)
            {
                if (beamOfLightObject)
                { beamOfLightObject.SetActive(hasLight); }

                if (reflectedLightTarget)
                {
                    reflectedLightTarget.ReceiveLight(false);
                }
            }

            if (renderer)
            {
                if (on && onColor)
                {
                    if (onColor)
                    {
                        renderer.material = onColor;
                    }
                }
                else
                {
                    if (offColor)
                    {
                        renderer.material = offColor;
                    }
                }
            }
        }

        public void CheckInFront()
        {
            if (raycastFrom == null) { return; }

            //Only cast against the target layer and obstacles. The player and any diagetic items shouldnt interfere with the puzzle
            LayerMask mask = LayerMask.GetMask(Layer.ToInteract, Layer.Obstacle);
            RaycastHit hit;

            LightReflector reflectorHit = null;

            if (Physics.SphereCast(raycastFrom.position, spherecastRadius, transform.forward, out hit, spherecastDistance, mask))
            {
                reflectorHit = hit.transform.GetComponent<LightReflector>();
            }
            else
            {
            }

            if (reflectorHit)
            {
                if (!HasLight())
                {
                    if (reflectorHit.HasLight())
                    {
                        reflectorHit.CheckInFront();
                    }

                    return;
                }

                if (reflectedLightTarget)
                {
                    if (reflectorHit != reflectedLightTarget)
                    {
                        //if this rotated to a different reflector, turn off the previous one
                        reflectedLightTarget.ReceiveLight(false);
                    }
                }

                if (reflectorHit.HasLight())
                {
                    return;
                }

                //if the reflector isnt already on
                reflectorHit.ReceiveLight(true);
                reflectedLightTarget = reflectorHit;
            }
            else
            {
                if (reflectedLightTarget)
                {
                    reflectedLightTarget.ReceiveLight(false);
                }
                reflectedLightTarget = null;
            }

            SetBeamOfLight();
        }

        public void SetBeamOfLight()
        {
            if (!beamOfLightObject)
            { return; }

            beamOfLightObject.SetActive(HasLight());

            Vector3 newSize = new Vector3(1, 0, 1);
            if (reflectedLightTarget)
            { newSize = new Vector3(1, Vector3.Distance(reflectedLightTarget.transform.position, transform.position), 1); }
            else
            {
            }

            beamOfLightObject.transform.localScale = newSize;
        }

        public void CheckSolution()
        {
            CheckInFront();
            if (hasLight)
            {
                if (GetPuzzle())
                {
                    GetPuzzle().ApplySolution();
                }
            }
        }

        public bool HasLight()
        { return hasLight; }

        public bool IsSolved()
        {
            return hasLight;
        }
    }
}