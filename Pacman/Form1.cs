using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private Image playerImage;
        private Dictionary<string, Image> images;
        private int counter;
        private Timer timer;
        public LevelForm(Map levelMap)
        {
            images = PrepareImage();
            playerImage = images["pacman-left.png"];
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
                Text = "Lives: " + map.HealthPoints.ToString() + "; Score: " + map.Score.ToString() + "; Iteration: " + counter.ToString()
            };

            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                var image = Drawing(row, column);

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
            
            timer = new Timer() {Interval = 100};
            timer.Tick += (sender, args) =>
            {
                if (this == null)
                {
                    timer.Stop();
                    Close();
                }
                map.Update();
                DrawLevel(progressBar);
                if (map.IsGameOver)
                {
                    timer.Stop();
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            };
            timer.Start();
            KeyDown += (sender, args) =>
            {
                KeyHandler(args);
            };
        }

        public Dictionary<string, Image> PrepareImage()
        {
            var result = new Dictionary<string, Image>();
            var helpDir = Directory.GetCurrentDirectory();
            for (int j = 0; j < 2; j++)
            {
                while (helpDir[helpDir.Length - 1] != '\\')
                    helpDir = helpDir.Substring(0, helpDir.Length - 2);
                helpDir.Substring(0, helpDir.Length - 2);
            }
            var imagesDir = new DirectoryInfo(helpDir + @"Pacman\Sprites");
            foreach (var file in imagesDir.GetFiles("*.png"))
            {
                result.Add(file.Name, Image.FromFile(file.FullName));
            }
            return result;
        }

        Dictionary<Keys, MoveDirection> directions = new Dictionary<Keys, MoveDirection>
        {
            { Keys.A, MoveDirection.Left },
            { Keys.W, MoveDirection.Up },
            { Keys.S, MoveDirection.Down },
            { Keys.D, MoveDirection.Right }
        };

        private void KeyHandler(KeyEventArgs eventArgs)
        {
            var direction = MoveDirection.Down;
            if (directions.ContainsKey(eventArgs.KeyCode))
            {
                var moveDirection = directions[eventArgs.KeyCode];
                if (!map.IsPlayerBoost)
                    playerImage = images[$"pacman-{moveDirection.ToString().ToLower()}.png"];
                else
                    playerImage = images[$"pacman-{moveDirection.ToString().ToLower()}-angry.png"];
                direction = moveDirection;
            }

            map.SetPlayerMoveDirection(direction);
        }

        private void DrawLevel(Label progressBar)
        {
            counter++;
            if (counter % 30 == 0 && map.IsAttackMode == false)
                map.IsAttackMode = true;
            else if (counter % 30 == 0)
                map.IsAttackMode = false;
            progressBar.Text = "Lives: " + map.HealthPoints.ToString() + "; Score: " + map.Score.ToString() + "; Iteration: " + counter.ToString();
            var rowCount = map.Field.GetLength(0);
            var columnCount = map.Field.GetLength(1);
            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                var image = Drawing(row, column);

                ((PictureBox) table.GetControlFromPosition(column, row)).Image = image;

            }
        }

        private Image Drawing(int row, int column)
        {
            Image result;
            if (map.Field[row, column] is Player)
                result = playerImage;
            else if (map.Field[row, column] is Ghost)
                result = images["ghost.png"];
            else if (map.Field[row, column] is Wall)
                result = images["square.png"];
            else if (map.Field[row, column] is Coin)
                result = images["coin.png"];
            else if (map.Field[row, column] is BigCoin)
                result = images["bigCoin.png"];
            else
                result = images["white-square.png"];
            return result;
        }
    }
}