using DonutDiner.FrameworkModule;
using DonutDiner.FrameworkModule.Data;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    [ExecuteInEditMode]
    public class SpinningStatue : SerializableObject, IPuzzlePiece
    {
        public float debugcurrentAngleDifference;
        public bool debugTurnOn;
        public bool debugIsSolved;

        [SerializeField] private Puzzle puzzle;
        [SerializeField] private int rotationNeeded; //multiplied by the invrement [e.g. rotationNeeded = 1 checks for 45degrees]

        [SerializeField] private Vector3 rotationDirection = new Vector3(0, 1, 0);
        private float INCREMENT = 45; //constant: dealing in a set rotation amount to prevent clerical off by one puzzle problems

        [Header("Debug: Editor window shows ray in the 'solution' direction")]
        [SerializeField] private bool debugShowSolution;

        [SerializeField] private Color debugRayColor = Color.green;

        public void SetPuzzle(Puzzle newPuzzle)
        { puzzle = newPuzzle; }

        public bool IsSolved()
        {
            float angleDifference = AngleDifference();
            debugcurrentAngleDifference = angleDifference;
            //check the difference against half the increment to further lean in favor of the player if they check the solution while the statues are still moving
            if (Mathf.Abs(angleDifference) < INCREMENT)
            {
                return true;
            }
            return false;
        }

        public float AngleDifference()
        {
            float angleX = (transform.rotation.eulerAngles.x - (INCREMENT * rotationNeeded)) * rotationDirection.x;
            float angleY = (transform.rotation.eulerAngles.y - (INCREMENT * rotationNeeded)) * rotationDirection.y;
            float angleZ = (transform.rotation.eulerAngles.z - (INCREMENT * rotationNeeded)) * rotationDirection.z;

            return Mathf.Abs(angleX) + Mathf.Abs(angleY) + Mathf.Abs(angleZ);
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (debugShowSolution)
            {
                debugcurrentAngleDifference = transform.rotation.eulerAngles.y;
                Vector3 vec = Quaternion.AngleAxis((INCREMENT * rotationNeeded), rotationDirection) * Vector3.forward;
                Debug.DrawRay(transform.position, vec, debugRayColor);
            }
        }

        public void CheckSolution()
        {
            if (puzzle != null)
            { puzzle.TryToSolve(); }
        }
    }
}