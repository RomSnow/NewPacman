using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pacman.GameCore
{
    public partial class GameMenu : Form
    {
        string map;
        public GameMenu(string levelMap)
        {
            map = levelMap;
            this.MaximumSize = new Size(600, 400);
            this.MinimumSize = new Size(600, 400);
            var start = new Button
            {
                Location = new Point(ClientSize.Height/4 + 10, 30),
                Size = new Size(ClientSize.Height, 30),
                Text = "Start"
            };
            var exit = new Button
            {
                Location = new Point(ClientSize.Height / 4 + 10, 70),
                Size = new Size(ClientSize.Height, 30),
                Text = "Exit"
            };
            Controls.Add(start);
            Controls.Add(exit);

            start.Click += new EventHandler(startClick);
            exit.Click += new EventHandler(exitClick);
        }

        private void exitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startClick(object sender, EventArgs e)
        {
            var gameMap = new Map(map, 3);
            Form Game = new LevelForm(gameMap)
            {
                ClientSize = new Size(1000, 800)
            };
            Game.Show();
        }
    }
}
