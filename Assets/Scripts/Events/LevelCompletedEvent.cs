using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.Events
{
    public enum LevelCompleteStatus
    {
        Win,
        Lose
    }
    public struct LevelCompletedEvent : IEvent
    {
        public LevelCompleteStatus Status { get; }

        public LevelCompletedEvent(LevelCompleteStatus status)
        {
            Status = status;
        }
    }
}