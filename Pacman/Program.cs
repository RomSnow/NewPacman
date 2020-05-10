using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pacman.GameCore;

namespace Pacman
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            var map = new Map("#####\n#...#\n#.P.#\n#...#\n#####", 3);
            Application.Run(new LevelForm(map)
            {
                ClientSize = new Size(500, 400)
            });
        }
    }
}