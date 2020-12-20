using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eto.Forms;

namespace Tag.Common.Gui.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        private bool isLoggedIn { get; set; }
        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }
            set
            {
                if (isLoggedIn != value)
                {
                    isLoggedIn = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public Command LoginCommand => new Command(SomeHandler);
        void SomeHandler(object sender, EventArgs e)
        {
            Console.WriteLine(sender);
            Console.WriteLine(e);
            // do some logic, use LoginCommand.Enabled to enable/disable the command, etc.
        }
        void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            Console.WriteLine(this.Password);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}