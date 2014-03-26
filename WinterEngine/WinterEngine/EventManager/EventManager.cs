using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WinterEngine
{
    public class EventManager
    {
        //private List<IEventSendable> managerList;
        // trying with a dictionary
        //private Dictionary<Object, TypeOfEvent> managers;

        public delegate void SoundEventHandler(Object sender, SoundEventArgs sndEventArgs);
        public event SoundEventHandler PlaySound;

        public enum TypeOfEvent
        {
            SoundEvent,
            AIEvent,
            SceneEvent
        }

        public EventManager()
        {
            //managerList = new List<IEventSendable>();
            //managers = new Dictionary<object, TypeOfEvent>();
        }

        public void Update(GameTime gametime)
        {

        }

        public void AddListenerToList(IEventSubscriber listener, TypeOfEvent typeOfEvent)
        {
            //managerList.Add(listener);
            //managers.Add(listener, typeOfEvent);
        }

        public void FireSoundEvent(Object actor, SoundEventArgs sndEventArgs) 
        {
            if(PlaySound != null)
            {
                PlaySound(actor, sndEventArgs);
            }
        }
    }
}