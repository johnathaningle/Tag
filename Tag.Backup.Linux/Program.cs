using System;
using Eto.Forms;
using Tag.Backup.Linux;

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
