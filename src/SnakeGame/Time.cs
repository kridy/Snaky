using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SnakeGame
{
    internal class Time {

        private static readonly long frequency;
        private static readonly double multiplier = 1.0e9;
        static Time()
        {
            if (QueryPerformanceFrequency(out frequency) == false)
            {
                // Frequency not supported
                throw new Win32Exception();
            }
        }

        public static double GetNanoSeconds()
        {
            long counter;

            QueryPerformanceCounter(out counter);

            return ((double)counter * (double)multiplier) / (double)frequency;
        }

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    }
}