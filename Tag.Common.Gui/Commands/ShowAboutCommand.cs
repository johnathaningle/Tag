using System;
using Eto.Forms;
using Tag.Common;

namespace Tag.Common.Gui.Commands
{
    public class ShowAboutCommand : Command
    {
        public ShowAboutCommand()
		{
			MenuText = "About";
			//ToolBarText = "Click Me";
			//ToolTip = $"Tagging Gui Version: {Constants.Version}";
			//Image = Icon.FromResource ("MyResourceName.ico");
			//Image = Bitmap.FromResource ("MyResourceName.png");
			Shortcut = Application.Instance.CommonModifier | Keys.A;  // control+A or cmd+A
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			MessageBox.Show(Application.Instance.MainForm, $"Tagging Gui Version: {Constants.Version}", "An Open Source Software", MessageBoxButtons.OK);
		}
    }
}