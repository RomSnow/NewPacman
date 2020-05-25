using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pacman.GameCore;

namespace Pacman
{
    public partial class LevelForm : Form
    {
        private Map map;
        private TableLayoutPanel table;
        private string pathToPlayerImage = @"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\pacman-left.png";
        public LevelForm(Map levelMap)
        {
            map = levelMap;
            table = new TableLayoutPanel();
            var rowCount = map.Field.GetLength(0);
            var columnCount = map.Field.GetLength(1);
            
            for (var r = 0; r < rowCount; r++)
            for (var c = 0; c < columnCount; c++)
            {
                table.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100f / rowCount)
                    );
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / columnCount)
                    );
            }

            var progressBar = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 30),
                Text = "Lives: " + map.HealthPoints.ToString() + "\n Score: " + map.Score.ToString()
            };

            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                Image image;

                if (map.Field[row, column] is Player)
                    image = Image.FromFile(pathToPlayerImage);
                else if (map.Field[row, column] is Ghost)
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\ghost.png");
                else if (map.Field[row, column] is Wall)
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\квадрат.png");
                else
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\white_square.jpg");

                var picture = new PictureBox()
                {
                    Image = image,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                picture.Dock = DockStyle.Fill;
                table.Controls.Add(picture, column, row);
            }
            table.Padding = Padding.Empty;
            table.Dock = DockStyle.Fill;
            Controls.Add(progressBar);
            Controls.Add(table);
            
            var timer = new Timer() {Interval = 400};
            timer.Tick += (sender, args) =>
            {
                map.Update();
                DrawLevel();
            };
            timer.Start();
            KeyDown += (sender, args) =>
            {
                KeyHandler(args);
            };

        }

        private void KeyHandler(KeyEventArgs eventArgs)
        {
            var direction = MoveDirection.Down;
            if (eventArgs.KeyCode == Keys.A)
            {
                pathToPlayerImage = @"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\pacman-left.png";
                direction = MoveDirection.Left;
            }
            else if (eventArgs.KeyCode == Keys.W)
            {
                pathToPlayerImage = @"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\pacman-up.png";
                direction = MoveDirection.Up;
            }
            else if (eventArgs.KeyCode == Keys.S)
            {
                pathToPlayerImage = @"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\pacman-down.png";
                direction = MoveDirection.Down;
            }
            else if (eventArgs.KeyCode == Keys.D)
            {
                pathToPlayerImage = @"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\pacman-right.png";
                direction = MoveDirection.Right;
            }
            map.SetPlayerMoveDirection(direction);
        }

        private void DrawLevel()
        {
            var rowCount = map.Field.GetLength(0);
            var columnCount = map.Field.GetLength(1);
            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                Image image;

                if (map.Field[row, column] is Player)
                    image = Image.FromFile(pathToPlayerImage);
                else if (map.Field[row, column] is Ghost)
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\ghost.png");
                else if (map.Field[row, column] is Wall)
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\квадрат.png");
                else
                    image = Image.FromFile(@"C:\Users\20kol\source\repos\NewPacman\Pacman\Sprites\white_square.jpg");

                ((PictureBox) table.GetControlFromPosition(column, row)).Image = image;

            }
        }
    }
}