using Eto.Forms;
using Tag.Common.Gui.Models;
using Tag.Common.Gui.Forms;
using Tag.Common.Gui.Views;
using Tag.Common.Services;

namespace Tag.Common.Gui
{
    public class MainApplication : Application
    {
        public void Run(string[] args)
        {
            
            RegisterServices();
            this.Run(ServiceLocator.Current.Get<MainForm>());
        }
        public void RegisterServices()
        {
            //register forms
            ServiceLocator.Current.Register<MainForm>();
            //register views
            ServiceLocator.Current.Register<MainView>();
            //register models
            ServiceLocator.Current.Register<MainViewModel>();
            ServiceLocator.Current.Register<LoginModel>(() => new LoginModel());
            //register services
            ServiceLocator.Current.Register<UnitOfWork>();
        }
    }
}