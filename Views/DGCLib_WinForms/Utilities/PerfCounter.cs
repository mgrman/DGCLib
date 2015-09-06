using System;
using System.Diagnostics;

namespace DGCLib_WinForms.Utilities
{
    public class PerfCounter
    {
        private static PerfCounter _instance = null;

        /// <summary>
        /// Returns singleton instance of PerfCounter.
        /// </summary>
        public static PerfCounter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PerfCounter();

                return _instance;
            }
        }

        // http://www.codeproject.com/Articles/2635/High-Performance-Timer-in-C

        // http://stackoverflow.com/questions/1739259/how-to-use-queryperformancecounter

        private double PCFreq = 0.0;
        private Int64 CounterStart = 0;

        /// <summary>
        /// Constructor starts counter.
        /// </summary>
        private PerfCounter()
        {
            Int64 li;
            if (!NativeMethods.QueryPerformanceFrequency_w(out li))
                Debug.WriteLine("QueryPerformanceFrequency failed!");

            PCFreq = (double)li;

            NativeMethods.QueryPerformanceCounter_w(out li);
            CounterStart = li;
        }

        /// <summary>
        /// Property returns the actual counter value in seconds.
        /// </summary>
        public double Seconds
        {
            get
            {
                Int64 li;
                NativeMethods.QueryPerformanceCounter_w(out li);
                return ((double)(li - CounterStart)) / PCFreq;
            }
        }
    }
}