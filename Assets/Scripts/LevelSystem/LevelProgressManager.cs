using System;
using AgaveLinkCase.Events;
using AgaveLinkCase.EventSystem;
using AgaveLinkCase.LevelSystem.ScoreCalculation;

namespace AgaveLinkCase.LevelSystem
{
    public class LevelProgressManager : IDisposable
    {
        public int TargetScore { get; private set; }
        public int Score { get; private set; }
        public int MoveCount { get; private set; }
        public event Action<int> OnScoreValueChanged;
        public event Action<int> OnMoveCountValueChanged;
        public event Action OnLevelSuccess;
        public event Action OnLevelFail;

        private readonly IScoreCalculationStrategy _scoreCalculationStrategy;
        private readonly EventBinding<LinkCollectedEvent> _linkCollectBinding;
        
        public LevelProgressManager(int targetScoreCount, int initialMoveCount)
        {
            Score = 0;
            TargetScore = targetScoreCount;
            MoveCount = initialMoveCount;
            _scoreCalculationStrategy = new ExactScoreCalculationStrategy();
            
            _linkCollectBinding = new EventBinding<LinkCollectedEvent>(OnLinkCollected);
            EventBus<LinkCollectedEvent>.Subscribe(_linkCollectBinding);
            
        }
        public void Dispose()
        {
            EventBus<LinkCollectedEvent>.Unsubscribe(_linkCollectBinding);
        }
        private void OnLinkCollected(LinkCollectedEvent eventData)
        {
            UpdateScore(eventData.Count);
            DecrementMove();

            if (IsGameOver())
            {
                FinishGame();
            }
            
            // Removed due to case document says finish game when no move left
            // else if (IsScoreEnough())
            // {
            //     TriggerLevelWin();
            // }
        }
        
        private void UpdateScore(int collectedCount)
        {
            Score += _scoreCalculationStrategy.CalculateScore(collectedCount);
            OnScoreValueChanged?.Invoke(Score);
        }

        private void DecrementMove()
        {
            MoveCount--;
            OnMoveCountValueChanged?.Invoke(MoveCount);
        }

        private bool IsGameOver() => MoveCount <= 0;

        private bool IsScoreEnough() => Score >= TargetScore;

        private void FinishGame()
        {
            if (IsScoreEnough())
                TriggerLevelWin();
            else
                TriggerLevelFail();
        }

        private void TriggerLevelWin()
        {
            EventBus<LevelCompletedEvent>.Publish(new LevelCompletedEvent(LevelCompleteStatus.Win));
            OnLevelSuccess?.Invoke();
        }

        private void TriggerLevelFail()
        {
            EventBus<LevelCompletedEvent>.Publish(new LevelCompletedEvent(LevelCompleteStatus.Lose));
            OnLevelFail?.Invoke();
        }
        
    }
}