using System;

namespace AgaveLinkCase.EventSystem
{
    public interface IEventBinding<T> where T : IEvent
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
}