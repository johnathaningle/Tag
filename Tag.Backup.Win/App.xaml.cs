using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Tag.Backup.Win.Models;
using Tag.Common.Models;
using Tag.Common.Services;
using TagBackWin.ViewModels;
using TagBackWin.Views;

namespace TagBackWin {
    public class App : Application {
        public override void Initialize () {
            AvaloniaXamlLoader.Load (this);
        }

        public override void OnFrameworkInitializationCompleted () {
            // RegisterServices();
            // var uow = ServiceLocator.Current.Get<UnitOfWork>();
            // InitApplication(uow).Wait();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                desktop.MainWindow = new MainWindow {
                    DataContext = new MainWindowViewModel (),
                };
            }

            base.OnFrameworkInitializationCompleted ();
        }

        private async Task InitApplication(UnitOfWork uow)
        {
            var userCount = uow.UserRepository.Users_Get().Count();
            if(userCount == 0)
            {
                var u = new User {
                    Username = "admin",
                    Password = uow.CryptoRepository.EncryptString ("tagback"),
                };
                uow.UserRepository.Users_Add(u);
                await uow.SaveChangesAsync();
            }
        }

        public override void RegisterServices()
        {
            //services
            ServiceLocator.Current.Register<UnitOfWork>(() => new UnitOfWork());

            //viewmodel props
            ServiceLocator.Current.Register<CurrentUserModel>(() => new CurrentUserModel());
        }
    }
}