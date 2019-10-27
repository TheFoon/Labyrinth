using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    class Tile : Panel
    {
        public readonly int TileId;
        public int Treasure { get; set; }
        public bool PathUp { get; set; }
        public bool PathRight { get; set; }
        public bool PathDown { get; set; }
        public bool PathLeft { get; set; }

        //public Random random = new Random();


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
        }
    }
}
