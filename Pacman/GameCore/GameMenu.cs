using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
            var leaderboard = new Button
            {
                Location = new Point(ClientSize.Height / 4 + 10, 70),
                Size = new Size(ClientSize.Height, 30),
                Text = "Leaderboard"
            };
            var exit = new Button
            {
                Location = new Point(ClientSize.Height / 4 + 10, 110),
                Size = new Size(ClientSize.Height, 30),
                Text = "Exit"
            };

            Controls.Add(start);
            Controls.Add(leaderboard);
            Controls.Add(exit);

            start.Click += new EventHandler(startClick);
            leaderboard.Click += new EventHandler(leaderboardClick);
            exit.Click += new EventHandler(exitClick);
        }

        private void exitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void leaderboardClick(object sender, EventArgs e)
        {
            Form leaders = new LeaderBoardForm()
            {
                ClientSize = new Size(500, 900)
            };
            leaders.Show();
        }

        private void startClick(object sender, EventArgs e)
        {
            var gameMap = new Map(map, 3);
            Form Game = new LevelForm(gameMap)
            {
                ClientSize = new Size(1000, 800)
            };
            var r = Game.ShowDialog();
            if (r == DialogResult.OK)
            {
                Form GameOver = new GameOverMenu(gameMap)
                {
                    ClientSize = new Size(500, 300)
                };
                GameOver.ShowDialog();
            }
        }
    }
}
