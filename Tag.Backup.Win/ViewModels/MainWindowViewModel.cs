using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using Tag.Backup.Win.Models;
using Tag.Common.Services;

namespace TagBackWin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, string> OpenCommand { get; }
        public MainWindowViewModel(Func<Task<String>> openCommand)
        {
            OpenCommand = ReactiveCommand
                .CreateFromTask(openCommand, outputScheduler: RxApp.MainThreadScheduler);
        }
        public string Greeting
        {
            get
            {
                var username = ServiceLocator.Current.Get<CurrentUserModel>()?.User?.Username;
                if (string.IsNullOrEmpty(username))
                    return "Hello There";

                return "Welcome " + username;
            }
        }
    }
}
