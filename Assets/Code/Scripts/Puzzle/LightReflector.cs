using DonutDiner.FrameworkModule;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{

    public class LightReflector : InteractiveDevice, IPuzzlePiece
    {
        public bool debugTurnOn;
        public bool debugIsSolved;

        
        [SerializeField] private Puzzle puzzle;
        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;
        [SerializeField] private int maxRotationIncrements; //multiplied by the invrement [e.g. rotationNeeded = 1 checks for 45degrees]

        [SerializeField] private Transform raycastFrom;
        [SerializeField] private GameObject beamOfLightObject;
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Material offColor;
        [SerializeField] private Material onColor;

        private Vector3 rotationDirection = new Vector3(0, 1, 0);
        private float increment = 15;
        private float rotSpeed = 50;
        private float spherecastRadius = 0.1f;
        private float spherecastDistance = 10;
        [SerializeField] private float remainingRotation;
        private int incrementCount;
        private int direction = 1;

        private LightReflector reflectedLightTarget; //the reflector that this reflector is bouncing light onto

        public void SetPuzzle(Puzzle newPuzzle)
        { puzzle = newPuzzle; }

        public override void StartInteraction()
        {
          //  if (remainingRotation != 0) { return; }
            if (IsLocked()) return;
            if (direction == 0) { direction = 1; }
            incrementCount += 1;
            //if the count exceeds the rotation from origin change the direction
            if (incrementCount > maxRotationIncrements)
            {
                direction *= -1;
                incrementCount = -maxRotationIncrements;
            }

            remainingRotation =  increment;

        }


        public bool IsSolved()
        {
            debugIsSolved = IsActivated;
   
            return IsActivated;
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (remainingRotation != 0) { Spin(); }
            else
            {
                if (debugTurnOn) { debugTurnOn = false; StartInteraction(); }
            }


        }

        public void Spin()
        {
            

            float step = Time.deltaTime * rotSpeed;
            if (step > remainingRotation) { step = remainingRotation; }

            transform.Rotate(direction * rotationDirection * step);

            remainingRotation -= step;

            if (remainingRotation <= 0)
            {
                CheckInFront();
                IsSolved();
                if (puzzle) { puzzle.TryToSolve(); }

                return;
            }
        }


        public void ReceiveLight(bool on)
        {
            if (IsActivated == on) { return; }
            IsActivated = on;

            if (!on)
            {
                if (beamOfLightObject)
                { beamOfLightObject.SetActive(IsActivated); }

                

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

            LayerMask mask = LayerMask.GetMask("To Interact");
            RaycastHit hit;
            if (Physics.SphereCast(raycastFrom.position, spherecastRadius, transform.forward, out hit, spherecastDistance,mask))
            {
                LightReflector reflectorHit = hit.transform.GetComponent<LightReflector>();
                if (reflectorHit )
                {
                    if (!IsActivated)
                    {
                        if (reflectorHit.IsActivated)
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
                    if (reflectorHit.IsActivated)
                    {
                        return;
                    }

                    //if the reflector isnt already on 
                    reflectorHit.ReceiveLight(true);
                        reflectedLightTarget = reflectorHit;
                        SetBeamOfLight();
                        
                    
                }
            }
            else
            {
                if (reflectedLightTarget)
                {
                    reflectedLightTarget.ReceiveLight(false);
                }
                if (beamOfLightObject)
                { beamOfLightObject.SetActive(false); }
            }

        }

        public void SetBeamOfLight()
        {
            if (!beamOfLightObject)
            {return;  }

            beamOfLightObject.SetActive(IsActivated);

            if (!reflectedLightTarget)
            { return; }

            beamOfLightObject.transform.localScale = new Vector3(1, Vector3.Distance(reflectedLightTarget.transform.position, transform.position),1 );

        }


    }
}