using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Labyrinth
{
    class GameHandler
    {
        public enum PlayerStatus
        {
            None,
            Host,
            Joining,
            Connected
        }
        public PlayerStatus playerStatus = PlayerStatus.None;
        private BoardHandler boardHandler = new BoardHandler();
        public static List<IPAddress> playerIPs = new List<IPAddress>();

        public GameHandler()
        {

        }

        public void Start() {

        }

        //Start hosting game
        public void HostGame() {
            LobbyHandler.HostBroadcast(true);
            playerStatus = PlayerStatus.Host;
        }

        public void JoinGame() {
            LobbyHandler.RecieveHosts(true);
            playerStatus = PlayerStatus.Joining;
        }
    }

    class BoardHandler
    {
        //------------------------------
        internal class Board
        {
            public Panel[,] PlayingBoard { get; set; }
            public Panel FreeTile { get; set; }
            public Panel[] ControlPanelsColumns { get; set; }
            public Panel[] ControlPanelsRows { get; set; }

            Random random = new Random();

            /// <summary>
            /// Constructor for Board class objects
            /// </summary>
            /// <param name="size">Determines the size of the PlayingBoards</param>
            public Board(int size)
            {
                FreeTile = new Panel();
                FreeTile.Size = new Size(50, 50);
                FreeTile.AllowDrop = true;
                FreeTile.BorderStyle = BorderStyle.Fixed3D;

                PlayingBoard = new Panel[size, size];
                if (size == 7)
                {
                    Tile freetile = new Tile(50, random);
                    freetile.DetermineTilePicture();
                    freetile.Enabled = true;
                    FreeTile.Controls.Add(freetile);
                } 
                else
                {
                    Tile freetile = new Tile(82, random);
                    freetile.DetermineTilePicture();
                    freetile.Enabled = true;
                    FreeTile.Controls.Add(freetile);
                }
                FreeTile.Location = new Point(1100, 10);

                FreeTile.DragDrop += Panel_DragDrop;
            }

            private void Board_DragEnter(object sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
            }

            private void Panel_DragDrop(object sender, DragEventArgs e)
            {
                ((Tile)e.Data.GetData(typeof(Tile))).Parent = (Panel)sender;//3rd was Tile
            }



            /// <summary>
            /// Fills the PlayingBoard matrix with objects. Creates the FreeTile
            /// </summary>
            public void FillBoardWithTile()
            {
                for (int x = 0; x < PlayingBoard.GetLength(0); x++)
                {
                    for (int y = 0; y < PlayingBoard.GetLength(1); y++)
                    {
                        Panel panel = new Panel();
                        panel.Size = new Size(50, 50);
                        panel.AllowDrop = true;
                        panel.BorderStyle = BorderStyle.Fixed3D;
                        /*panel.DragEnter += Board_DragEnter;
                        panel.DragDrop += Panel_DragDrop;*/

                        PlayingBoard[x, y] = panel;

                        if (y % 2 == 0 && x % 2 == 0)
                        {
                            Tile tile = new Tile((y * PlayingBoard.GetLength(0)) + (x + 1), random);
                            if (tile.TileId == 1)
                            {
                                tile.PathRight = true; tile.PathDown = true;
                                tile.PathUp = false; tile.PathLeft = false;
                            }
                            else if (tile.TileId == PlayingBoard.GetLength(0))
                            {
                                tile.PathDown = true; tile.PathLeft = true;
                                tile.PathUp = false; tile.PathRight = false;  
                            }
                            else if (tile.TileId == (PlayingBoard.GetLength(0) * PlayingBoard.GetLength(0)) - (PlayingBoard.GetLength(0) - 1))
                            {
                                tile.PathUp = true; tile.PathRight = true;
                                tile.PathDown = false; tile.PathLeft = false;
                            }
                            
                            else if (tile.TileId == (PlayingBoard.GetLength(0) * PlayingBoard.GetLength(0)))
                            {
                                tile.PathUp = true; tile.PathLeft = true;
                                tile.PathRight = false; tile.PathDown = false;
                            }
                            
                            else if (y == 0)
                            {
                                tile.PathRight = true; tile.PathDown = true; tile.PathLeft = true;
                                tile.PathUp = false;
                            }
                            
                            else if (y == PlayingBoard.GetLength(0) - 1)
                            {
                                tile.PathUp = true; tile.PathRight = true; tile.PathLeft = true;
                                tile.PathDown = false;
                            }

                            else if (x == 0)
                            {
                                tile.PathUp = true; tile.PathRight = true; tile.PathDown = true;
                                tile.PathLeft = false;
                            }

                            else if (x == PlayingBoard.GetLength(1) - 1)
                            {
                                tile.PathUp = true; tile.PathDown = true; tile.PathLeft = true;
                                tile.PathRight = false;
                            }
                            tile.Size = new Size(50, 50);
                            tile.DetermineTilePicture();

                            PlayingBoard[x, y].Enabled = false;

                            PlayingBoard[x, y].Controls.Add(tile);
                        }
                        else
                        {
                            Tile tile = new Tile((y * PlayingBoard.GetLength(0)) + (x + 1), random);
                            tile.Size = new Size(50, 50);

                            tile.DetermineTilePicture();

                            PlayingBoard[x, y].Controls.Add(tile);
                        }
                    }
                }
            }

            /// <summary>
            /// Places the Tile objects from the PlayingBoard matrix
            /// </summary>
            /// <param name="c">Which control the objects should be added</param>
            public void PlaceTiles(Control c)
            {
                int y = 70, x = 70;
                for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                    {
                        if (PlayingBoard[i, j] != null)
                        {
                            c.Controls.Add(PlayingBoard[i, j]);
                            PlayingBoard[i, j].Location = new Point(y, x);
                            x += 60;
                        }
                        else
                        {
                            x += 60;
                        }
                    }
                    x = 70;
                    y += 60;
                }
                c.Controls.Add(FreeTile);
            }

            public void PlaceGameControls(Control control)
            {
                if (PlayingBoard.GetLength(0) == 7)
                {
                    int y = 10, x = 130;

                    Panel column1_top = new Panel(), column2_top = new Panel(), column3_top = new Panel(), column1_bottom = new Panel(), column2_bottom = new Panel(), column3_bottom = new Panel();
                    ControlPanelsColumns = new Panel[] { column1_top, column2_top, column3_top, column1_bottom, column2_bottom, column3_bottom};

                    for (int i = 0; i < 2; i++)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (i != 0 && j == 0)
                            {
                                j = 3;
                            }
                            ControlPanelsColumns[j].AllowDrop = true;
                            ControlPanelsColumns[j].Size = new Size(50, 50);
                            ControlPanelsColumns[j].BorderStyle = BorderStyle.Fixed3D;
                            ControlPanelsColumns[j].Location = new Point(x, y);
                            ControlPanelsColumns[j].DragEnter += Board_DragEnter;
                            ControlPanelsColumns[j].DragDrop += Panel_DragDrop;

                            control.Controls.Add(ControlPanelsColumns[j]);

                            x += 120;
                            j++;

                            if (j % 3 == 0 && (j != 3 || i == 0))
                                break;
                        }
                        x = 130;
                        y += 480;
                    }

                    y = 130; x = 10;

                    Panel row1_top = new Panel(), row2_top = new Panel(), row3_top = new Panel(), row1_bottom = new Panel(), row2_bottom = new Panel(), row3_bottom = new Panel();
                    ControlPanelsRows = new Panel[] { row1_top, row2_top, row3_top, row1_bottom, row2_bottom, row3_bottom };

                    for (int i = 0; i < 2; i++)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (i != 0 && j == 0)
                            {
                                j = 3;
                            }
                            ControlPanelsRows[j].AllowDrop = true;
                            ControlPanelsRows[j].Size = new Size(50, 50);
                            ControlPanelsRows[j].BorderStyle = BorderStyle.Fixed3D;
                            ControlPanelsRows[j].Location = new Point(x, y);
                            ControlPanelsRows[j].DragEnter += Board_DragEnter;
                            ControlPanelsRows[j].DragDrop += Panel_DragDrop;

                            control.Controls.Add(ControlPanelsRows[j]);

                            y += 120;
                            j++;

                            if (j % 3 == 0 && (j != 3 || i == 0))
                                break;
                        }
                        y = 130;
                        x += 480;
                    }
                }

                else
                {
                    int y = 10, x = 130;

                    Panel column1_top = new Panel(), column2_top = new Panel(), column3_top = new Panel(), column4_top = new Panel(), column1_bottom = new Panel(), column2_bottom = new Panel(), column3_bottom = new Panel(), column4_bottom = new Panel();
                    ControlPanelsColumns = new Panel[] { column1_top, column2_top, column3_top, column4_top, column1_bottom, column2_bottom, column3_bottom, column4_bottom };

                    for (int i = 0; i < 2; i++)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (i != 0 && j == 0)
                            {
                                j = 4;
                            }
                            ControlPanelsColumns[j].AllowDrop = true;
                            ControlPanelsColumns[j].Size = new Size(50, 50);
                            ControlPanelsColumns[j].BorderStyle = BorderStyle.Fixed3D;
                            ControlPanelsColumns[j].Location = new Point(x, y);
                            ControlPanelsColumns[j].DragEnter += Board_DragEnter;
                            ControlPanelsColumns[j].DragDrop += Panel_DragDrop;

                            control.Controls.Add(ControlPanelsColumns[j]);

                            x += 120;
                            j++;

                            if (j % 4 == 0 && (j != 4 || i == 0))
                                break;
                        }
                        x = 130;
                        y += 600;
                    }

                    y = 130; x = 10;

                    Panel row1_top = new Panel(), row2_top = new Panel(), row3_top = new Panel(), row4_top = new Panel(), row1_bottom = new Panel(), row2_bottom = new Panel(), row3_bottom = new Panel(), row4_bottom = new Panel();
                    ControlPanelsRows = new Panel[] { row1_top, row2_top, row3_top, row4_top, row1_bottom, row2_bottom, row3_bottom, row4_bottom };

                    for (int i = 0; i < 2; i++)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (i != 0 && j == 0)
                            {
                                j = 4;
                            }
                            ControlPanelsRows[j].AllowDrop = true;
                            ControlPanelsRows[j].Size = new Size(50, 50);
                            ControlPanelsRows[j].BorderStyle = BorderStyle.Fixed3D;
                            ControlPanelsRows[j].Location = new Point(x, y);
                            ControlPanelsRows[j].DragEnter += Board_DragEnter;
                            ControlPanelsRows[j].DragDrop += Panel_DragDrop;

                            control.Controls.Add(ControlPanelsRows[j]);

                            y += 120;
                            j++;

                            if (j % 4 == 0 && (j != 4 || i == 0))
                                break;
                        }
                        y = 130;
                        x += 600;
                    }
                }

                Button button = new Button();
                button.Size = new Size(50, 50);
                button.Location = new Point(1100, 70);
                button.Text = "OK";
                button.Click += button_Click;

                control.Controls.Add(button);
            }

            private void button_Click(object sender, EventArgs e)
            {
                throw new NotImplementedException();
            }
        }
        //------------------------------
    }

    public static class LobbyHandler
    {
        private static IPAddress IP_ADDRESS;
        private static IPAddress IP_BROADCAST;

        static LobbyHandler() {
            string info = GetIP();
            IP_ADDRESS = IPAddress.Parse(info.Split(' ')[0]);
            IPAddress mask = IPAddress.Parse(info.Split(' ')[1]);
            IP_BROADCAST = GetBroadcastAddress(IP_ADDRESS, mask);
        }

        public static void HostBroadcast(bool a) {

        }

        public static void RecieveHosts(bool a)
        {

        }

        private static string GetIP()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.OperationalStatus.ToString() == "Up" && adapter.GetIPProperties().DnsSuffix.ToString() == "home")
                {
                    foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString() + " " + ip.IPv4Mask.ToString();
                        }
                    }
                }
            }
            return "255.255.255.255";
        }

        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
