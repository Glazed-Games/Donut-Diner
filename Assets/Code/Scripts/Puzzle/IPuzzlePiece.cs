namespace DonutDiner.FrameworkModule
{
    public interface IPuzzlePiece
    {
        public void SetPuzzle(Puzzle newPuzzle);

        public bool IsSolved();
    }
}