using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.Events
{
    public struct LinkCollectedEvent : IEvent
    {
        public int Count { get; }

        public LinkCollectedEvent(int count)
        {
            Count = count;
        }
    }
}