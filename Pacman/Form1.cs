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
                Text = "Lives: " + map.HealthPoints.ToString() + "\nScore: " + map.Score.ToString()
            };

            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                Image image;

                if (map.Field[row, column] is Player)
                    image = playerImage;
                else if (map.Field[row, column] is Ghost)
                    image = images["ghost.png"];
                else if (map.Field[row, column] is Wall)
                    image = images["квадрат.png"];
                else if (map.Field[row, column] is Coin)
                    image = images["coin.png"];
                else if (map.Field[row, column] is BigCoin)
                    image = images["bigCoin.png"];
                else
                    image = images["white_square.png"];

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
            
            var timer = new Timer() {Interval = 100};
            timer.Tick += (sender, args) =>
            {
                map.Update();
                DrawLevel(progressBar);
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

        private void KeyHandler(KeyEventArgs eventArgs)
        {
            var direction = MoveDirection.Down;
            if (eventArgs.KeyCode == Keys.A)
            {
                playerImage = images["pacman-left.png"];
                direction = MoveDirection.Left;
            }
            else if (eventArgs.KeyCode == Keys.W)
            {
                playerImage = images["pacman-up.png"];
                direction = MoveDirection.Up;
            }
            else if (eventArgs.KeyCode == Keys.S)
            {
                playerImage = images["pacman-down.png"];
                direction = MoveDirection.Down;
            }
            else if (eventArgs.KeyCode == Keys.D)
            {
                playerImage = images["pacman-right.png"];
                direction = MoveDirection.Right;
            }
            map.SetPlayerMoveDirection(direction);
        }

        private void DrawLevel(Label progressBar)
        {
            progressBar.Text = "Lives: " + map.HealthPoints.ToString() + "\nScore: " + map.Score.ToString();
            var rowCount = map.Field.GetLength(0);
            var columnCount = map.Field.GetLength(1);
            for (var row = 0; row < rowCount; row++)
            for (var column = 0; column < columnCount; column++)
            {
                Image image;

                if (map.Field[row, column] is Player)
                    image = playerImage;
                else if (map.Field[row, column] is Ghost)
                    image = images["ghost.png"];
                else if (map.Field[row, column] is Wall)
                    image = images["квадрат.png"];
                else if (map.Field[row, column] is Coin)
                    image = images["coin.png"];
                else if (map.Field[row, column] is BigCoin)
                    image = images["bigCoin.png"];
                else
                    image = images["white_square.png"];

                ((PictureBox) table.GetControlFromPosition(column, row)).Image = image;

            }
        }
    }
}