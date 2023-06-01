using DonutDiner.FrameworkModule;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    [ExecuteInEditMode]
    public class SpinningStatue : InteractiveDevice, IPuzzlePiece
    {
        public bool debugTurnOn;
        public bool debugIsSolved;

        [SerializeField] private Puzzle puzzle;
        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;
        [SerializeField] private int rotationNeeded; //multiplied by the invrement [e.g. rotationNeeded = 1 checks for 45degrees]
        
        private Vector3 rotationDirection = new Vector3(0, 1, 0);
        private float increment = 45;
        private float rotSpeed = 50;
        private float remainingRotation;

        [Header("Debug: Editor window shows ray in the 'solution' direction")]
        [SerializeField] private bool debugShowSolution;
        [SerializeField] private Color debugRayColor = Color.green;


        public void SetPuzzle(Puzzle newPuzzle)
        { puzzle = newPuzzle; }

        public override void StartInteraction()
        {
            if (IsLocked()) return;

            if (!UnlockConditionMet()) return;

            IsActivated = !IsActivated;
            //if the statue is already rotating skip adding to it
            remainingRotation += increment;
        }

        public bool UnlockConditionMet()
        {
            //check a central keyItemSpot for the required item
            if (keySpot != null && !keySpot.HasKey()) return false;
            //check the player's inventory for the required item
            if (itemNeeded != null && !PlayerInventory.Instance.CheckForItem(itemNeeded)) return false;

            //if a key spot or a needed item isnt set, the device defaults to usable
            return true;
        }

        public bool IsSolved()
        {
            debugIsSolved = false;
            Debug.Log("EULER: " + transform.rotation.eulerAngles.y);
            if (Mathf.RoundToInt(Mathf.Abs(transform.rotation.eulerAngles.y)) == increment * rotationNeeded)
            {
                debugIsSolved = true;
                return true;
            }
            return false;
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (remainingRotation != 0) { Spin(); }
            if (IsActivated) { }
            else
            {
                if (debugTurnOn) { debugTurnOn = false; StartInteraction(); }
            }

            if (debugShowSolution)
            {
                Vector3 vec = Quaternion.AngleAxis(  (increment * rotationNeeded), rotationDirection) * transform.forward;
                Debug.DrawRay(transform.position, vec, debugRayColor);
            }

        }

        public void Spin()
        {
            if (remainingRotation <= 0)
            {
                IsSolved();
                if (puzzle) { puzzle.TryToSolve(); }
                IsActivated = !IsActivated;
                return;
            }

            float step = Time.deltaTime * rotSpeed;
            if (step > remainingRotation) { step = remainingRotation; }

            transform.Rotate(rotationDirection * step);

            remainingRotation -= step;
        }
    }
}