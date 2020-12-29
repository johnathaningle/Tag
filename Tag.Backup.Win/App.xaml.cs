using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Tag.Backup.Win.Models;
using Tag.Common.Models;
using Tag.Common.Services;
using TagBackWin.ViewModels;
using TagBackWin.Views;

namespace TagBackWin
{
    public class App : Application
    {
        public override void Initialize()
        {
            LocateServices();
            var uow = ServiceLocator.Current.Get<UnitOfWork>();
            InitApplication(uow).Wait();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.DataContext = new MainWindowViewModel(() =>
                {
                    return new OpenFolderDialog().ShowAsync(desktop.MainWindow);
                });
            }

            base.OnFrameworkInitializationCompleted();
        }

        private async Task InitApplication(UnitOfWork uow)
        {
            var userCount = uow.UserRepository.Users_Get().Count();
            if (userCount == 0)
            {
                var u = new User
                {
                    Username = "admin",
                    Password = uow.CryptoRepository.EncryptString("tagback"),
                };
                uow.UserRepository.Users_Add(u);
                await uow.SaveChangesAsync();

                var currentUser = uow.UserRepository.Users_Get()
                    .Where(x => x.Username == "admin")
                    .FirstOrDefault();
                ServiceLocator.Current.Set<CurrentUserModel>(new CurrentUserModel()
                {
                    User = currentUser,
                    BackupDirectories = new List<string>()
                });
            }
            else if (userCount > 0)
            {
                var currentUser = uow.UserRepository.Users_Get()
                    .FirstOrDefault();
                ServiceLocator.Current.Set<CurrentUserModel>(new CurrentUserModel()
                {
                    User = currentUser,
                    BackupDirectories = new List<string>()
                });
            }
        }

        public void LocateServices()
        {
            //services
            ServiceLocator.Current.Register<UnitOfWork>(() => new UnitOfWork());

            //viewmodel props
            ServiceLocator.Current.Register<CurrentUserModel>(() => new CurrentUserModel());
        }
    }
}