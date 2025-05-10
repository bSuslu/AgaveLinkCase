using UnityEngine;

namespace AgaveLinkCase.LevelSystem.ScoreCalculation
{
    public class ExponentialScoreStrategy
    {
        public int CalculateScore(int collectedLinkCount)
        {
            return (int)Mathf.Pow(2, collectedLinkCount);
        }
    }
}