using System;

namespace WinterEngine
{
    public interface IEventSubscriber
    {
        void EventFired(Object sender, WinterEventArgs firedEvent);
    }
}
