using AgaveLinkCase.EventSystem;

namespace AgaveLinkCase.Events
{
    public class LinkCollectedEvent : IEvent
    {
        public int Count { get; }

        public LinkCollectedEvent(int count)
        {
            Count = count;
        }
    }
}