using System;

namespace DGCLib_WinForms.Utilities
{
    public class InterAppComunication
    {
        #region Singleton

        private static InterAppComunication _instance;

        public static InterAppComunication Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InterAppComunication();
                return _instance;
            }
        }

        #endregion Singleton

        private InterAppComunication()
        {
        }

        #region Log

        public event EventHandler<LogMessageArgs> Log;

        public static void ReportLog(string message)
        {
            var handler = Instance.Log;
            if (handler != null)
            {
                handler(Instance, new LogMessageArgs(message));
            }
        }

        public static void ReportLog(string message, params object[] args)
        {
            var handler = Instance.Log;
            if (handler != null)
            {
                handler(Instance, new LogMessageArgs(string.Format(message, args)));
            }
        }

        #endregion Log

        #region Error

        public event EventHandler<ErrorMessageArgs> Error;

        public static void ReportError(string message)
        {
            ReportError(message, null);
        }

        public static void ReportError(string message, Exception ex)
        {
            var handler = Instance.Error;
            if (handler != null)
            {
                handler(Instance, new ErrorMessageArgs(message, ex));
            }
        }

        #endregion Error

        #region YesNo question

        public Func<string, bool> QuestionHandler { get; set; }

        public static bool AskYesNoQuestion(string question)
        {
            if (Instance.QuestionHandler != null)
            {
                return Instance.QuestionHandler(question);
            }

            return false;
        }

        #endregion YesNo question
    }

    public class ErrorMessageArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public string GUIMessage { get; set; }

        public ErrorMessageArgs(string message, Exception ex)
        {
            GUIMessage = message;
            Exception = ex;
        }

        public ErrorMessageArgs(string message)
        {
            GUIMessage = message;
            Exception = null;
        }
    }

    public class LogMessageArgs : EventArgs
    {
        public string Message { get; set; }

        public LogMessageArgs(string message, Exception ex)
        {
            Message = message;
        }

        public LogMessageArgs(string message)
        {
            Message = message;
        }
    }
}