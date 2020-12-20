using Eto.Drawing;
using Eto.Forms;
using Tag.Common.Gui.Commands;

namespace Tag.Common.Gui.Forms
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            BuildMenu();
        }
        private void BuildMenu()
        {
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem
                    {
                        Text = "File",
                        Items =
                        {
							// you can add commands or menu items
							new ShowAboutCommand(),
                            new ButtonMenuItem { Text = "Preferences" }
                        }
                    }
                },
                // quit item (goes in Application menu on OS X, File menu for others)
                QuitItem = new Command((sender, e) => Application.Instance.Quit())
                {
                    MenuText = "Quit",
                    Shortcut = Application.Instance.CommonModifier | Keys.Q
                },
                // about command (goes in Application menu on OS X, Help menu for others)
                AboutItem = new Command((sender, e) => new Dialog
                {
                    Content = new Label
                    {
                        Text = "About my app..."
                    },
                    ClientSize = new Size(200, 200)
                }.ShowModal(this))
                {
                    MenuText = "About my app"
                }
            };
        }
    }


}