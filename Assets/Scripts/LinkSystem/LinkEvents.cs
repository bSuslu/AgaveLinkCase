using System.Collections.Generic;
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

    public struct LinkSuccessEvent : IEvent
    {
        public List<ILinkable> Link { get; }
        
        public LinkSuccessEvent(List<ILinkable> link)
        {
            Link = link;
        }
    }
}