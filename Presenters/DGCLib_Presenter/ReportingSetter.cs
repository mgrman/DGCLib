using DGCLib_Base;
using System;

namespace DGCLib_Presenter
{
    public class ReportingSetter
    {
        public Func<double> TimeProvider
        {
            get
            {
                return Reporting.TimeProvider;
            }

            set
            {
                Reporting.TimeProvider = value;
            }
        }

        public ReportingSetter()
        {
            Reporting.MessageAdded += (o, e) => FireMessageAdded(e.Message);
        }

        public event EventHandler<MessageEventArgs> MessageAdded;

        private void FireMessageAdded(string message)
        {
            var handler = MessageAdded;
            if (handler != null)
                handler(this, new MessageEventArgs(message));
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}