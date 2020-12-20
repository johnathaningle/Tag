using Eto.Forms;

namespace Tag.Common.Gui.Forms
{
    public class HomeForm : BaseForm
    {
        public HomeForm()
        {
            Content = new Label() { Text = "Hello World" };
        }
    }
}