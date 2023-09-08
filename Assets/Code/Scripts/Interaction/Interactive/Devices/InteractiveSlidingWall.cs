using DonutDiner.FrameworkModule;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using UnityEngine;
using UnityEngine.Events;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    public class InteractiveSlidingWall : InteractiveDevice
    {
        public bool debugTurnOn;
        public bool debugIsSolved;

        [SerializeField] private IPuzzlePiece puzzlePiece;
        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;



        [SerializeField] private Vector3 slideDirection = new Vector3(0, 1, 0);
        private Vector3 startPoint;
        private Vector3 targetPoint;
        [SerializeField] private float distance = 1;
        [SerializeField] private float speed = 150;

        private int incrementCount;
        private int direction = 1;

        [Header("Assign a puzzle piece to check 'IsSolved() if this controls a puzzle element")]
        [SerializeField] private UnityEvent OnFinish;



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
            //the start point is where the object slides from
            if (startPoint == Vector3.zero) { startPoint = transform.position; }

            if (transform.position != startPoint)
            {
                targetPoint = startPoint;
            }
            else { targetPoint = startPoint + (direction * distance * slideDirection); }



        }

        private void Start()
        {
            targetPoint = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.position != targetPoint)
            { 
                Move();
            }
            else
            {
                if (debugTurnOn) { debugTurnOn = false; StartInteraction(); }
            }
        }

        public void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPoint,Time.deltaTime * speed);


            if (transform.position == targetPoint)
            {
                if (OnFinish != null)
                {
                    OnFinish.Invoke();
                }
                if (GetPuzzle() != null) { GetPuzzle().IsSolved(); }

            }
        }
    }
}