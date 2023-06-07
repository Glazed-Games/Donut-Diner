using DonutDiner.InteractionModule.Interactive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonutDiner.FrameworkModule
{
    public class BlacklightPuzzle : Puzzle
    {
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {

        }

        public override void TryToSolve()
        {
            Solved = false;
            foreach (IPuzzlePiece el in Pieces())
            {
                if (!el.IsSolved()) return;
            }

            ApplySolution();
        }

        public override void ApplySolution()
        {
            if (LockedObject() != null)
            {
                if (LockedObject().GetComponent<IInteractive>() != null)
                {
                    LockedObject().GetComponent<IInteractive>().IsInteractable(true);
                    Solved = true;
                }
            }
        }

    }
}