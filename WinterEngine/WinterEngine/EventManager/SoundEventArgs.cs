using Microsoft.Xna.Framework.Audio;

namespace WinterEngine
{
    public class SoundEventArgs : WinterEventArgs
    {
        SoundEffect sFx;
        double volume;
        int repeatNumberOfTimes;

        public SoundEventArgs(double volume, int repeatNumberOfTimes)
        {
            //this.sFx = sFx;
            this.volume = volume;
            this.repeatNumberOfTimes = repeatNumberOfTimes;
        }
    }
}
