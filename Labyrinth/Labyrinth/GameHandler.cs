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
using System.Diagnostics;

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
        //Board class and its methods
        internal class Board
        {
            public Panel[,] PlayingBoard { get; set; }
            public Panel FreeTile { get; set; }
            public Panel[] ControlPanelsColumns { get; set; }
            public Panel[] ControlPanelsRows { get; set; }
            public Player[] Players { get; set; }

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

                Players = new Player[4];

                //If the board is 7*7
                if (size == 7)
                {
                    Tile freetile = new Tile(random);
                    freetile.DetermineTilePicture();
                    freetile.Enabled = true;
                    FreeTile.Controls.Add(freetile);
                } 

                //If the board is 9*9
                else
                {
                    Tile freetile = new Tile(random);
                    freetile.DetermineTilePicture();
                    freetile.Enabled = true;
                    FreeTile.Controls.Add(freetile);
                }
                FreeTile.Location = new Point(1100, 10);

                FreeTile.DragDrop += Panel_DragDrop;
            }

            /// <summary>
            /// Panel Drag Enter Event handler
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Board_DragEnter(object sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
            }

            /// <summary>
            /// Panel Drag and Drop Event Handler
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Panel_DragDrop(object sender, DragEventArgs e)
            {
                ((Tile)e.Data.GetData(typeof(Tile))).Parent = (Panel)sender;//3rd was Tile
            }

            /// <summary>
            /// Player objects' DragDrop Event handler
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Player_DragDrop(object sender, DragEventArgs e)
            {
                ((Player)e.Data.GetData(typeof(Player))).Parent = (Panel)sender;//3rd was Tile
                
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

                        panel.DragEnter += Board_DragEnter;
                        panel.DragDrop += Player_DragDrop;

                        PlayingBoard[x, y] = panel;

                        //Sets the properties of the fixed tiles (e.g: starting locations)
                        if (y % 2 == 0 && x % 2 == 0)
                        {
                            Tile tile = new Tile(random);

                            #region Starting Locations
                            if ((y * PlayingBoard.GetLength(0)) +(x + 1) == 1)
                            {
                                tile.PathRight = true; tile.PathDown = true;
                                tile.PathUp = false; tile.PathLeft = false;
                            }
                            else if ((y * PlayingBoard.GetLength(0)) + (x + 1) == PlayingBoard.GetLength(0))
                            {
                                tile.PathDown = true; tile.PathLeft = true;
                                tile.PathUp = false; tile.PathRight = false;  
                            }
                            else if ((y * PlayingBoard.GetLength(0)) + (x + 1) == (PlayingBoard.GetLength(0) * PlayingBoard.GetLength(0)) - (PlayingBoard.GetLength(0) - 1))
                            {
                                tile.PathUp = true; tile.PathRight = true;
                                tile.PathDown = false; tile.PathLeft = false;
                            }
                            
                            else if ((y * PlayingBoard.GetLength(0)) + (x + 1) == (PlayingBoard.GetLength(0) * PlayingBoard.GetLength(0)))
                            {
                                tile.PathUp = true; tile.PathLeft = true;
                                tile.PathRight = false; tile.PathDown = false;
                            }
                            #endregion

                            #region Other Fix Tiles
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
                            #endregion

                            tile.Size = new Size(50, 50);
                            tile.DetermineTilePicture();

                            //PlayingBoard[x, y].Enabled = false;

                            PlayingBoard[x, y].Controls.Add(tile);
                        }
                        //Adds the non-fixed tiles to the board matrix
                        else
                        {
                            Tile tile = new Tile(random);
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
            /// <param name="control">Which control the objects should be added</param>
            public void PlaceTiles(Control control)
            {
                int y = 70, x = 70;
                for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                    {
                        if (PlayingBoard[i, j] != null)
                        {
                            control.Controls.Add(PlayingBoard[i, j]);
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
                control.Controls.Add(FreeTile);
            }

            /// <summary>
            /// Places some controls needed for gameplay
            /// </summary>
            /// <param name="control">Which control the object should be added</param>
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

                    Panel row1_left = new Panel(), row2_left = new Panel(), row3_left = new Panel(), row1_right = new Panel(), row2_right = new Panel(), row3_right = new Panel();
                    ControlPanelsRows = new Panel[] { row1_left, row2_left, row3_left, row1_right, row2_right, row3_right };

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

                    Panel row1_left = new Panel(), row2_left = new Panel(), row3_left = new Panel(), row4_left = new Panel(), row1_right = new Panel(), row2_right = new Panel(), row3_right = new Panel(), row4_right = new Panel();
                    ControlPanelsRows = new Panel[] { row1_left, row2_left, row3_left, row4_left, row1_right, row2_right, row3_right, row4_right };

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
                button.Location = new Point(1100, 160);
                button.Text = "OK";
                button.Click += button_Click;

                control.Controls.Add(button);
            }

            /// <summary>
            /// Makes the specific columns shift
            /// </summary>
            /// <param name="which_colmun"></param>
            /// <param name="top">Whether the column shift should happen from the top or the bottom</param>
            /// <param name="panel">Which control panel has the freetile placed in it</param>
            public void ShiftBoardColumns(int which_colmun, bool top, Panel panel)
            {
                Control[] panel_array = new Control[1]; //Was Tile

                //Clones the tile, then sets the 'Enabled' property to false then removes the tile from the panel's control then add the edited tile
                Tile tile = (Tile)panel.Controls[0];
                tile.Enabled = false;
                panel.Controls.RemoveAt(0);
                panel.Controls.Add(tile);

                panel.Controls.CopyTo(panel_array, 0);

                
                List<Control[]> tiles = new List<Control[]>(); //Was TIle 
                
                //Fills a list with the copies of the Tile objects inside the panels
                for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                    {
                        if (j == which_colmun)
                        {
                            Control[] array = new Control[4]; //Was Tile
                            PlayingBoard[j, i].Controls.CopyTo(array, 0);
                            tiles.Add(array);
                        }
                    }
                }
                
                //If the free tile was placed in a top column control panel
                if (top)
                {
                    for (int i = 1; i < PlayingBoard.GetLength(0); i++)
                    {
                        for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                        {
                            if (j == which_colmun)
                            {
                                PlayingBoard[j, i].Controls.Clear();
                                PlayingBoard[j, i].Controls.AddRange(tiles[i - 1]);
                            }
                        }
                    }
                    PlayingBoard[which_colmun, 0].Controls.Clear();
                    PlayingBoard[which_colmun, 0].Controls.AddRange(panel_array);

                    FreeTile.Controls.Clear();

                    tiles.Last().First().Enabled = true;

                    FreeTile.Controls.AddRange(tiles.Last());

                    panel.Controls.Clear();
                }
                
                //If the free tile was placed in a bottom column control panel
                else
                {
                    for (int i = PlayingBoard.GetLength(0) - 1; i > 0; i--)
                    {
                        for (int j = PlayingBoard.GetLength(1); j > 0; j--)
                        {
                            if (j == which_colmun)
                            {
                                PlayingBoard[j, i - 1].Controls.Clear();
                                PlayingBoard[j, i - 1].Controls.AddRange(tiles[i]);

                            }
                        }
                    }
                    PlayingBoard[which_colmun, PlayingBoard.GetLength(0) - 1].Controls.Clear();
                    PlayingBoard[which_colmun, PlayingBoard.GetLength(0) - 1].Controls.AddRange(panel_array);

                    FreeTile.Controls.Clear();

                    tiles.First().First().Enabled = true;

                    FreeTile.Controls.AddRange(tiles.First());

                    panel.Controls.Clear();
                }
                
            }

            /// <summary>
            /// Makes the specific row shift
            /// </summary>
            /// <param name="which_row"></param>
            /// <param name="left">Whether the row shift should happen from the left or the right</param>
            /// <param name="panel">Which control panel has the freetile placed in it</param>
            public void ShiftBoardRows(int which_row, bool left, Panel panel)
            {
                Control[] panel_array = new Control[1]; //Was Tile

                //Clones the tile, then sets the 'Enabled' property to false then removes the tile from the panel's control then add the edited tile
                Tile tile = (Tile)panel.Controls[0];
                tile.Enabled = false;
                panel.Controls.RemoveAt(0);
                panel.Controls.Add(tile);

                panel.Controls.CopyTo(panel_array, 0);
                List<Control[]> tiles = new List<Control[]>(); // Was Tile
                
                //Fills a list with the copies of the Tile objects inside the panels
                for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                    {
                        if (i == which_row)
                        {
                            Control[] array = new Control[4]; //Was Tile
                            PlayingBoard[j, i].Controls.CopyTo(array, 0);
                            tiles.Add(array);
                        }
                    }
                }

                //If the free tile was placed in a right row control panel
                if (left)
                {
                    for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                    {
                        for (int j = 1; j < PlayingBoard.GetLength(1); j++)
                        {
                            if (i == which_row)
                            {
                                PlayingBoard[j, i].Controls.Clear();
                                PlayingBoard[j, i].Controls.AddRange(tiles[j - 1]);
                            }
                        }
                    }
                    PlayingBoard[0, which_row].Controls.Clear();
                    PlayingBoard[0, which_row].Controls.AddRange(panel_array);

                    FreeTile.Controls.Clear();

                    tiles.Last().Last().Enabled = true;

                    FreeTile.Controls.AddRange(tiles.Last());

                    panel.Controls.Clear();
                }

                //If the free tile was placed in a left row control panel
                else
                {
                    for (int i = PlayingBoard.GetLength(0); i > 0; i--)
                    {
                        for (int j = PlayingBoard.GetLength(1) - 1; j > 0; j--)
                        {
                            if (i == which_row)
                            {
                                PlayingBoard[j - 1, i].Controls.Clear();
                                PlayingBoard[j - 1, i].Controls.AddRange(tiles[j]);

                            }
                        }
                    }
                    PlayingBoard[PlayingBoard.GetLength(0) - 1, which_row].Controls.Clear();
                    PlayingBoard[PlayingBoard.GetLength(0) - 1, which_row].Controls.AddRange(panel_array);

                    FreeTile.Controls.Clear();

                    tiles.First().First().Enabled = true;

                    FreeTile.Controls.AddRange(tiles.First());

                    panel.Controls.Clear();
                }
            }

            /// <summary>
            /// Button click event handler
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void button_Click(object sender, EventArgs e)
            {
                //Check if the free tile was placed in a cloumn control panel
                for (int i = 0; i < ControlPanelsColumns.Length; i++)
                {
                    if (ControlPanelsColumns[i].Controls.Count != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                ShiftBoardColumns(1, true, ControlPanelsColumns[i]);
                                break;
                            case 1:
                                ShiftBoardColumns(3, true, ControlPanelsColumns[i]);

                                break;
                            case 2:
                                ShiftBoardColumns(5, true, ControlPanelsColumns[i]);

                                break;
                            case 3:
                                if (ControlPanelsColumns.Length > 7)
                                {
                                    ShiftBoardColumns(7, true, ControlPanelsColumns[i]);

                                }
                                else
                                {
                                    ShiftBoardColumns(1, false, ControlPanelsColumns[i]);

                                }

                                break;
                            case 4:
                                if (ControlPanelsColumns.Length > 7)
                                {
                                    ShiftBoardColumns(1, false, ControlPanelsColumns[i]);

                                }
                                else
                                {
                                    ShiftBoardColumns(3, false, ControlPanelsColumns[i]);

                                }

                                break;
                            case 5:
                                if (ControlPanelsColumns.Length > 7)
                                {
                                    ShiftBoardColumns(3, false, ControlPanelsColumns[i]);

                                }
                                else
                                {
                                    ShiftBoardColumns(5, false, ControlPanelsColumns[i]);

                                }

                                break;
                            case 6:
                                ShiftBoardColumns(5, false, ControlPanelsColumns[i]);

                                break;
                            case 7:
                                ShiftBoardColumns(7, false, ControlPanelsColumns[i]);

                                break;
                            default:
                                break;
                        }
                    }
                }

                //Check if the free tile was placed in a row control panel
                for (int i = 0; i < ControlPanelsRows.Length; i++)
                {

                    if (ControlPanelsRows[i].Controls.Count != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                ShiftBoardRows(1, true, ControlPanelsRows[i]);
                                break;
                            case 1:
                                ShiftBoardRows(3, true, ControlPanelsRows[i]);

                                break;
                            case 2:
                                ShiftBoardRows(5, true, ControlPanelsRows[i]);

                                break;
                            case 3:
                                if (ControlPanelsRows.Length > 7)
                                {
                                    ShiftBoardRows(7, true, ControlPanelsRows[i]);

                                }
                                else
                                {
                                    ShiftBoardRows(1, false, ControlPanelsRows[i]);

                                }

                                break;
                            case 4:
                                if (ControlPanelsRows.Length > 7)
                                {
                                    ShiftBoardRows(1, false, ControlPanelsRows[i]);

                                }
                                else
                                {
                                    ShiftBoardRows(3, false, ControlPanelsRows[i]);

                                }

                                break;
                            case 5:
                                if (ControlPanelsRows.Length > 7)
                                {
                                    ShiftBoardRows(3, false, ControlPanelsRows[i]);

                                }
                                else
                                {
                                    ShiftBoardRows(5, false, ControlPanelsRows[i]);

                                }

                                break;
                            case 6:
                                ShiftBoardRows(5, false, ControlPanelsRows[i]);

                                break;
                            case 7:
                                ShiftBoardRows(7, false, ControlPanelsRows[i]);

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            /// <summary>
            /// Creates the set number of Player objects
            /// </summary>
            /// <param name="numofPlyers">Number of players</param>
            public void AddPlayersToBoard(int numofPlyers)
            {
                for (int i = 0; i < numofPlyers; i++)
                {
                    Player player;
                    switch (i)
                    {
                        case 0:
                            player = new Player(0, 0); Players[i] = player; PlayingBoard[0, 0].Controls.Add(Players[i]); player.BringToFront();
                            break;

                        case 1:
                            player = new Player(PlayingBoard.GetLength(0) - 1, 0); Players[i] = player; PlayingBoard[PlayingBoard.GetLength(0) - 1, 0].Controls.Add(Players[i]); player.BringToFront();
                            break;

                        case 2:
                            player = new Player(0, PlayingBoard.GetLength(1) - 1); Players[i] = player; PlayingBoard[0, PlayingBoard.GetLength(1) - 1].Controls.Add(Players[i]); player.BringToFront();
                            break;

                        case 3:
                            player = new Player(PlayingBoard.GetLength(0) - 1, PlayingBoard.GetLength(1) - 1); Players[i] = player; PlayingBoard[PlayingBoard.GetLength(0) - 1, PlayingBoard.GetLength(1) - 1].Controls.Add(Players[i]); player.BringToFront();
                            break;

                        default:
                            break;
                    }
                }
            }

            public bool CheckIfPlayerMovementValid(int playerX, int playerY, int playerDesiredX, int playerDesiredY)
            {
                bool valid = false;

                
                
                return valid;
            }
        }
    }

    public static class LobbyHandler
    {
        private static IPAddress IP_ADDRESS;
        private static IPAddress IP_BROADCAST;
        private static int PORTNUMBER = 14999;
        private static bool hosting = false;
        private static bool recieving = false;
        private static UdpClient udpc = new UdpClient(PORTNUMBER);

        static LobbyHandler() {
            string info = GetIP();
            IP_ADDRESS = IPAddress.Parse(info.Split(' ')[0]);
            IPAddress mask = IPAddress.Parse(info.Split(' ')[1]);
            IP_BROADCAST = GetBroadcastAddress(IP_ADDRESS, mask);
        }

        public static void HostBroadcast(bool a) {
            if (a)
            {
                hosting = true;
                Task.Run(() => {
                    while (hosting) {
                        string message = $"hb;4;"; //host broadcast ; 
                        UdpClient client = new UdpClient();
                        IPEndPoint ip = new IPEndPoint(IP_BROADCAST, PORTNUMBER);
                        byte[] bytes = Encoding.ASCII.GetBytes(message);
                        client.Send(bytes, bytes.Length, ip);
                        client.Close();
                    }
                });
            }
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
