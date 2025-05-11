namespace LevelSystem.ScoreCalculation
{
    public class ExactScoreCalculationStrategy : IScoreCalculationStrategy
    {
        public int CalculateScore(int collectedLinkCount)
        {
            return collectedLinkCount;
        }
    }
}