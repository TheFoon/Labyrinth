using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    class Player : PictureBox
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public int CurrentTileId { get; set; }
        public int Inventory { get; set; }
        public int[] Objectives { get; set; }

        /// <summary>
        /// Constructor for Player class objects
        /// </summary>
        public Player(/*team color which will decide the starting tile's coordinates*/)
        {



            Inventory = 0;


            Objectives = new int[3];
        }
    }
}
