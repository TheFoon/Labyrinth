using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Labyrinth
{
    class Player : Label
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public int Inventory { get; set; }
        public readonly int[] HomeTile;

        /// <summary>
        /// Constructor for Player class objects
        /// </summary>
        public Player(int playerHomeX, int playerHomeY)
        {
            HomeTile = new int[] { playerHomeX, playerHomeY};
            CoordinateX = playerHomeX;
            CoordinateY = playerHomeY;
            Inventory = 0;

            Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../figure_transparent.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(0); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            MouseDown += Player_MouseDown;
        }

        private void Player_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(this, DragDropEffects.Move);
            BringToFront();
        }

    }
}
