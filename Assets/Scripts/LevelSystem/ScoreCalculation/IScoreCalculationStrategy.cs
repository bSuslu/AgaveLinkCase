namespace AgaveLinkCase.LevelSystem.ScoreCalculation
{
    public interface IScoreCalculationStrategy
    {
        public int CalculateScore(int collectedLinkCount);
    }
}