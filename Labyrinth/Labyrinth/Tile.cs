using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Labyrinth
{
    class Tile : Label
    {
        public readonly int TileId;
        public int Treasure { get; set; }
        public bool PathUp { get; set; }
        public bool PathRight { get; set; }
        public bool PathDown { get; set; }
        public bool PathLeft { get; set; }

        /// <summary>
        /// Constructor for Tile class objects
        /// </summary>
        /// <param name="id">The Tile objects id</param>
        /// <param name="r">Required for the random generation of paths</param>
        public Tile(int id, Random r)
        {
            TileId = id;
            int numbof_paths = r.Next(2, 4), current_numbof_paths = 0;
            while (current_numbof_paths != numbof_paths)
            {
                // Randomly decides whether the Tile should have a path up
                if (r.Next(0, 2) == 1 && !PathUp && (current_numbof_paths != numbof_paths))
                {
                    PathUp = true;
                    current_numbof_paths++;
                }

                // Randomly decides whether the Tile should have a path to the right
                if (r.Next(0, 2) == 1 && !PathRight && (current_numbof_paths != numbof_paths))
                {
                    PathRight = true;
                    current_numbof_paths++;
                }

                // Randomly decides whether the Tile should have a path down
                if (r.Next(0, 2) == 1 && !PathDown && (current_numbof_paths != numbof_paths))
                {
                    PathDown = true;
                    current_numbof_paths++;
                }

                // Randomly decides whether the Tile should have a path to the left
                if (r.Next(0, 2) == 1 && !PathLeft && (current_numbof_paths != numbof_paths))
                {
                    PathLeft = true;
                    current_numbof_paths++;
                }

                // Add treasure to tile with a 20% chance
                if (r.Next(0, 101) < 80)
                    Treasure = 1;
                else
                    Treasure = 0;
            }
            MouseDown += L_MouseDown;
        }

        /// <summary>
        /// Determines which image should be assigned to the path
        /// </summary>
        public void DetermineTilePicture()
        {
            if (this.PathUp && !this.PathRight && this.PathDown && !this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../straight.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(0); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (!this.PathUp && this.PathRight && !this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../straight.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(90); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (this.PathUp && this.PathRight && !this.PathDown && !this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../turn.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(270); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (!this.PathUp && this.PathRight && this.PathDown && !this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../turn.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(0); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (!this.PathUp && !this.PathRight && this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../turn.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(90); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (this.PathUp && !this.PathRight && !this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../turn.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(180); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (this.PathUp && this.PathRight && this.PathDown && !this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../t_path.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(90); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (!this.PathUp && this.PathRight && this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../t_path.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(180); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (this.PathUp && !this.PathRight && this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../t_path.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(270); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };

            else if (this.PathUp && this.PathRight && !this.PathDown && this.PathLeft)
                Paint += (s, ev) => { Graphics g = ev.Graphics; Image i = new Bitmap(@"../../t_path.png"); g.TranslateTransform((float)i.Width / 2, (float)i.Height / 2); g.RotateTransform(0); g.TranslateTransform(-(float)i.Width / 2, -(float)i.Height / 2); g.DrawImage(i, new Point(0, 0)); };
        }

        private void L_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(this, DragDropEffects.Move);
        }

        private void TIle_DragDrop(object sender, DragEventArgs e)
        {
            ((Label)e.Data.GetData(typeof(Label))).Parent = (Panel)sender;//3rd was Tile
        }

        private void Tile_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
