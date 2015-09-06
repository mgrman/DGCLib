using System;

namespace DGCLib_Base
{
    public static class Reporting
    {
        private static DateTime _startTime;

        static Reporting()
        {
            _startTime = DateTime.Now;
        }

        public static Func<double> TimeProvider { get; set; }

        /// <summary>
        /// returns precise time
        /// </summary>
        /// <returns>seconds since start</returns>
        public static double Seconds
        {
            get
            {
                if (TimeProvider == null)
                    return (DateTime.Now - _startTime).TotalSeconds;

                return TimeProvider();
            }
        }

        public static event EventHandler<DomainMessageEventArgs> MessageAdded;

        public static void LogMessage(string message)
        {
            var handler = MessageAdded;
            if (handler != null)
                handler(null, new DomainMessageEventArgs(message));
        }

        public static void LogMessage(string format, params object[] args)
        {
            LogMessage(string.Format(format, args));
        }
    }

    public class DomainMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public DomainMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}