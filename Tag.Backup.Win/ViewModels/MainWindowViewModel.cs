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
    public class MainWindowViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> OpenCommand { get; }
        public MainWindowViewModel(Func<Task<String>> openCommand)
        {
            backupDirectories = new List<string>();
            OpenCommand = ReactiveCommand
                .CreateFromTask(AddDirectory(openCommand), outputScheduler: RxApp.MainThreadScheduler);
        }

        private Func<Task> AddDirectory(Func<Task<String>> openCommand)
        {
            return async () => {
                var directory = await Task.Run(openCommand);
                var u = ServiceLocator.Current.Get<CurrentUserModel>();
                backupDirectories.Add(directory);
                u.BackupDirectories = backupDirectories;
            };
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

        private List<string> backupDirectories;
        public List<string> BackupDirectories
        {
            get
            {
                var u = ServiceLocator.Current.Get<CurrentUserModel>();
                backupDirectories = u?.BackupDirectories ?? new List<string>();
                return backupDirectories;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref backupDirectories, value);
            }
        }
    }
}
