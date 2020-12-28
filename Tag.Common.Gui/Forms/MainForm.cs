
using Eto.Drawing;
using Eto.Forms;
using Tag.Common.Gui.Commands;
using Tag.Common.Gui.Layouts;
using Tag.Common.Gui.Models;
using Tag.Common.Gui.Views;
using Tag.Common.Services;

namespace Tag.Common.Gui.Forms
{
    public class MainForm : BaseForm
    {
        private MainView mainView;
        private MainViewModel mainVm;
        public MainForm()
        {
            mainView = ServiceLocator.Current.Get<MainView>();
            mainVm = ServiceLocator.Current.Get<MainViewModel>();
            this.DataContext = mainVm;
            // create menu
            Content = mainView.Content;
        }
    }
}