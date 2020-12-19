using System;
using Eto.Forms;
using Tag.Common.Gui.Views;

namespace TagBack
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application().Run(new HomeView());
        }
    }
}
