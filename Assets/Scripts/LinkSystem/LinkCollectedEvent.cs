using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.LinkSystem
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