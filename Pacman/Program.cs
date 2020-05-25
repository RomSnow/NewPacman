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
            var mapString = "######################\n" +
                            "#*...#..........#....#\n" +
                            "#.##.#.########.#.##.#\n" +
                            "#.#................#.#\n" +
                            "#.#.##.###..###.##.#.#\n" + 
                            "#.#....#G....G#....#.#\n" + 
                            "#...##.########.##...#\n" + 
                            "#.#................#.#\n" +
                            "#.#.##.########.##.#.#\n" +
                            "#.#................#.#\n" +
                            "#.##.#.########.#.##.#\n" +
                            "#...*#....PR....#...*#\n" +
                            "######################";
            
            Application.Run(new GameMenu(mapString)
            {
                ClientSize = new Size(500, 300)
            });
        }
    }
}