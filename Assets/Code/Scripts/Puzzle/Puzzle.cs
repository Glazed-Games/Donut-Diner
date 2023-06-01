using DonutDiner.FrameworkModule.Data;
using DonutDiner.InteractionModule.Interactive;
using System.Collections.Generic;
using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public class Puzzle : SerializableObject
    {
        [SerializeField] private bool solved;
        [SerializeField] private GameObject lockedObj;
        [SerializeField] private List<IPuzzlePiece> puzzlePieces;

        // Start is called before the first frame update
        private void Start()
        {
            //set the list of puzzle pieces, if the list is currently empty it checks it's transforms children for IPuzzlePiece
            Pieces();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public virtual void TryToSolve()
        {
            solved = false;
            foreach (IPuzzlePiece el in Pieces())
            {
                if (!el.IsSolved()) return;
            }

            ApplySolution();
        }

        public virtual void ApplySolution()
        {
            if (lockedObj != null)
            {
                if (lockedObj.GetComponent<IInteractive>() != null)
                {
                    lockedObj.GetComponent<IInteractive>().IsInteractable(true);
                    solved = true;
                }
            }
        }

        public List<IPuzzlePiece> Pieces()
        {
            if (puzzlePieces == null || puzzlePieces.Count == 0)
            {
                puzzlePieces = new List<IPuzzlePiece>();
                foreach (Transform el in transform)
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
                }
            }

            return puzzlePieces;
        }
    }
}