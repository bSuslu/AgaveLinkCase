namespace LevelSystem.ScoreCalculation
{
    public interface IScoreCalculationStrategy
    {
        public int CalculateScore(int collectedLinkCount);
    }
}