using DonutDiner.FrameworkModule;
using UnityEngine;

namespace DonutDiner.ItemModule.Items
{
    public class ItemToInputInto : MonoBehaviour, IPuzzlePiece
    {
        #region Fields

        [SerializeField] private Puzzle _puzzle;
        [SerializeField] private string _unlockCode;
        [SerializeField] private string lastCodeEntered;

        #endregion Fields



        #region Unity Methods

        private void Awake()
        {
        }

        #endregion Unity Methods



        #region Private Methods

        public void GetTextInput(string code)
        {
            lastCodeEntered = code;
            if (_puzzle) { _puzzle.TryToSolve(); }
        }

        public void SetPuzzle(Puzzle newPuzzle)
        {
            _puzzle = newPuzzle;
        }

        public bool IsSolved()
        {
            return lastCodeEntered == _unlockCode;
        }

        public void CheckSolution()
        {
            if (_puzzle) { _puzzle.TryToSolve(); }
        }

        #endregion Private Methods
    }
}