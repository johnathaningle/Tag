using System;

namespace Tag.Common.Gui.Events
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string message, string loglevel = "INFO")
        {
            Message = message;
            LogLevel = loglevel;
            TimeStamp = DateTime.Now;
        }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string LogLevel { get; set; }
    }
}