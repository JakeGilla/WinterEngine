using System;

namespace WinterEngine.Scenes
{
    class SceneManager : IEventSubscriber
    {
        void IEventSubscriber.EventFired(Object sender, WinterEventArgs firedEvent)
        {
            Console.WriteLine("Object: {0}", sender.ToString());
            Console.WriteLine("Event {1}", firedEvent.GetType().ToString());
        }
    }
}
