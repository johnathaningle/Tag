using System;
using Tag.Common.Gui.Events;

namespace Tag.Common.Gui.Models
{
    public class CoreViewModel : BaseViewModel
    {
        public event EventHandler<LogEventArgs> LogReceived;

        protected void OnLogRecieved(object message, string logLevel = "INFO")
        {
            LogReceived?.Invoke(this, new LogEventArgs(message?.ToString(), logLevel));
        }
    }
}