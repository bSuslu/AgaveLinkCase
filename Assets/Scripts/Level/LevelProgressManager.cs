using System;
using AgaveLinkCase.Events;
using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.Level
{
    public class LevelProgressManager : IDisposable
    {
        public int TargetScore { get; private set; }
        public int Score { get; private set; }
        public int MoveCount { get; private set; }
        public event Action<int> OnScoreValueChanged;
        public event Action<int> OnMoveCountValueChanged;

        private EventBinding<LinkCollectedEvent> _linkCollectBinding;
        public LevelProgressManager(int targetScoreCount, int initialMoveCount)
        {
            Score = 0;
            TargetScore = targetScoreCount;
            MoveCount = initialMoveCount;
            
            _linkCollectBinding = new EventBinding<LinkCollectedEvent>(OnLinkCollected);
            EventBus<LinkCollectedEvent>.Subscribe(_linkCollectBinding);
            
        }
        public void Dispose()
        {
            EventBus<LinkCollectedEvent>.Unsubscribe(_linkCollectBinding);
        }
        private void OnLinkCollected(LinkCollectedEvent eventData)
        {
            Score += CalculateScore(eventData.Count);
            OnScoreValueChanged?.Invoke(Score);
            if (Score >= TargetScore)
            {
                EventBus<LevelCompletedEvent>.Publish(new LevelCompletedEvent(LevelCompleteStatus.Win));
                return;
            }
            
            
            MoveCount--;
            OnMoveCountValueChanged?.Invoke(MoveCount);
            if (MoveCount <= 0)
            {
                EventBus<LevelCompletedEvent>.Publish(new LevelCompletedEvent(LevelCompleteStatus.Lose));
                return;
            }
        }
        private int CalculateScore(int collectedLinkCount)
        {
            //TODO
            return collectedLinkCount;
        }
    }
}