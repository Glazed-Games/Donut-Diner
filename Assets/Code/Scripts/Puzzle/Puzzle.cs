using DonutDiner.FrameworkModule.Data;
using DonutDiner.InteractionModule.Interactive;
using System.Collections.Generic;
using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public class Puzzle : SerializableObject
    {
        [SerializeField] private bool _solved;
        public bool Solved { get => _solved; set => _solved = value; }

        [SerializeField] private GameObject lockedObj;
        [SerializeField] private List<IPuzzlePiece> puzzlePieces;

        //[Header(" ")]
        //[SerializeField] private UnityEvent OnSolved;
        //[SerializeField] private UnityEvent OnUnSolved;

        // Start is called before the first frame update
        private void Start()
        {
            //set the list of puzzle pieces, if the list is currently empty it checks it's transforms children for IPuzzlePiece
            Pieces();
        }

        public virtual void TryToSolve()
        {
            _solved = true;
            foreach (IPuzzlePiece el in Pieces())
            {
                Debug.Log("Puzzle: " + el.IsSolved());
                if (!el.IsSolved()) { _solved = false; }
            }
            if (!_solved) { return; }
            ApplySolution();
        }

        public virtual void ApplySolution()
        {
            if (LockedObject() != null)
            {
                if (LockedObject().GetComponent<IInteractive>() != null)
                {
                    LockedObject().GetComponent<IInteractive>().StartInteraction();
                    Solved = true;
                }
            }
        }

        public GameObject LockedObject()
        { return lockedObj; }

        public void SearchChildTransforms(Transform childTransform)
        {
            foreach (Transform el in childTransform)
            {
                IPuzzlePiece newPiece = el.GetComponent<IPuzzlePiece>();
                if (newPiece != null)
                {
                    if (!puzzlePieces.Contains(newPiece))
                    {
                        puzzlePieces.Add(newPiece);
                        newPiece.SetPuzzle(this);
                    }
                }
                if (el.childCount > 0) { SearchChildTransforms(el); }
            }
        }

        public List<IPuzzlePiece> Pieces()
        {
            if (puzzlePieces == null || puzzlePieces.Count == 0)
            {
                puzzlePieces = new List<IPuzzlePiece>();
                SearchChildTransforms(transform);
            }

            return puzzlePieces;
        }
    }
}