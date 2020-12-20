
using Eto.Drawing;
using Eto.Forms;
using Tag.Common.Gui.Commands;
using Tag.Common.Gui.Layouts;
using Tag.Common.Gui.Models;
using Tag.Common.Gui.Services;
using Tag.Common.Gui.Views;

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
            Title = "TagBack";
            ClientSize = new Size(800, 600);
            // create menu
            Content = mainView.Content;
        }
    }
}