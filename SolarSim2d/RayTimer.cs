using Raylib_cs;
using System.Numerics;

namespace SolarSim2d
{
    public class RayTimer
    {
        double time, initTime;
        public RayTimer(double time)
        {
            this.time = time;
            initTime = time;
        }

        public RayTimer(){}

        public void UpdateTimer()
        {
            time -= Raylib.GetFrameTime();
        }

        public void ResetTimer()
        {
            time = initTime;
        }

        public bool CheckTimer()
        {
            if (time <= 0) return false;
            else return true;
        }
    }
}
