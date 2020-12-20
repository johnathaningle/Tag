using Eto.Forms;
using Tag.Common.Gui.Models;
using Tag.Common.Gui.Services;
using Tag.Common.Gui.Forms;

namespace Tag.Common.Gui
{
    public class MainApplication : Application
    {
        public void Run(string[] args)
        {
            RegisterServices();
            this.Run(ServiceLocator.Current.Get<HomeForm>());
        }
        public void RegisterServices()
        {
            //register views
            ServiceLocator.Current.Register<HomeForm>();
            //register models
            ServiceLocator.Current.Register<MainViewModel>();
            
        }
    }
}