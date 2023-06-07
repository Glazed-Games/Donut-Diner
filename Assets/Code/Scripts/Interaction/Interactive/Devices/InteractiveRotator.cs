using DonutDiner.FrameworkModule;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using UnityEngine;
using UnityEngine.Events;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    public class InteractiveRotator : InteractiveDevice
    {
        public bool debugTurnOn;
        public bool debugIsSolved;

        [SerializeField] private IPuzzlePiece puzzlePiece;
        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;
        [SerializeField] private int maxRotationIncrements; //multiplied by the increment [e.g. rotationNeeded = 1 checks for 45degrees]

        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Material offColor;
        [SerializeField] private Material onColor;

        private Vector3 rotationDirection = new Vector3(0, 1, 0);
        private float increment = 45;
        private float rotSpeed = 50;
        [SerializeField] private float remainingRotation;
        private int incrementCount;
        private int direction = 1;

        [Header("Assign a puzzle piece to check 'IsSolved() if this rotator controls a puzzle element")]
        [SerializeField] private UnityEvent OnFinish;

        [Header("Debug: Editor window shows ray in the 'solution' direction")]
        [SerializeField] private bool debugShowSolution;

        [SerializeField] private Color debugRayColor = Color.green;

        public void SetPuzzle(IPuzzlePiece newPuzzle)
        {
            //  puzzlePiece = newPuzzle;
        }

        public IPuzzlePiece GetPuzzle()
        {
            if (puzzlePiece == null) { puzzlePiece = GetComponent<IPuzzlePiece>(); }
            return puzzlePiece;
        }

        public override void StartInteraction()
        {
            //  if (remainingRotation != 0) { return; }
            if (IsLocked()) return;
            if (direction == 0) { direction = 1; }

            //Increments for objects that dont spin 360 degrees to see if they should rotate back the opposite direction
            //if the count exceeds the rotation from origin change the direction
            if (maxRotationIncrements != 0)
            {
                incrementCount += 1;

                if (incrementCount > maxRotationIncrements)
                {
                    direction *= -1;
                    incrementCount = -maxRotationIncrements;
                }
            }

            remainingRotation = increment;
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
                if (OnFinish != null)
                {
                    OnFinish.Invoke();
                }
                if (GetPuzzle() != null) { GetPuzzle().IsSolved(); }

                return;
            }
        }
    }
}