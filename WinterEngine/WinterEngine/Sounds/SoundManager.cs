using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using WinterEngine;


namespace WinterEngine.Sounds
{
    public class SoundManager : IEventSubscriber
    {
        // change back from public after debug!
        public List<object> senders;
        public List<SoundEventArgs> eventArgs;
        AudioEngine audioEngine;
        SoundBank soundBank;

        public SoundManager()
        {
            eventArgs = new List<SoundEventArgs>();
            senders = new List<object>(); // is this needed?
        }

        protected void Initialize()
        {
            try
            {
                //audioEngine = new AudioEngine()
                //soundBank = new SoundBank()
            }
            catch (Exception e)
            {

            }
        }

        public void SubscribeToHandler(EventManager em)
        {
            //em.PlaySound += new EventManager.SoundEventHandler(EventFired);
            // lambda expression
            em.PlaySound += (sender, firedEvent) => 
            {
                senders.Add(sender);
                eventArgs.Add(firedEvent);
            };
        }

        public void EventFired(object sender, WinterEventArgs firedEvent)
        {
            throw new NotImplementedException();
        }
    }
}
