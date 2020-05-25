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
    public partial class LeaderBoardForm : Form
    {
        public LeaderBoardForm()
        {
            var leaders = new Leaderboard();
            var l = new Label()
            {
                Size = new Size(100, 400),
                Text = leaders.GetLeaderboard()
            };
            Controls.Add(l);
        }
    }
}
