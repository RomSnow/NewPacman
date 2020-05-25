using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pacman.GameCore
{
    public partial class GameOverMenu : Form
    {
        private Map map;
        public GameOverMenu(Map map)
        {
            this.map = map;
            var score = map.Score;
            this.MinimumSize = new Size(300, 300);
            this.MaximumSize = new Size(300, 300);
            this.Text = "PACMAN";

            var info = new Label()
            {
                Location = new Point(ClientSize.Width/3 + 5, ClientSize.Height/3 - 40),
                Size = new Size(150, 100),
                Text = "Game Over!\nYour score: " + score + "\nEnter your name:"
            };
            var name = new TextBox()
            {
                Location = new Point(ClientSize.Width / 3, ClientSize.Height / 3 + 15),
                Size = new Size(110, 100)
            };
            var enter = new Button()
            {
                Location = new Point(ClientSize.Width / 3 + 25, ClientSize.Height / 3 + 45),
                Size = new Size(60, 25),
                Text = "Enter"
            };

            Controls.Add(enter);
            Controls.Add(name);
            Controls.Add(info);

            enter.Click += (sender, args) =>
            {
                enterClick(name.Text);
                this.Close();
            };
        }

        private void enterClick(string name)
        {
            var l = new Leaderboard();
            l.AddResult(name, map.Score);
        }
    }
}
