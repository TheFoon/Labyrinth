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
            public Tile[,] PlayingBoard { get; set; }
            public Tile FreeTile { get; set; }

            Random random = new Random();

            /// <summary>
            /// Constructor for Board class objects
            /// </summary>
            /// <param name="size">Determines the size of the PlayingBoards</param>
            public Board(int size)
            {
                PlayingBoard = new Tile[size, size];
                if (size == 7)
                    FreeTile = new Tile(50, random);
                else
                    FreeTile = new Tile(82, random);
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
                            tile.Size = new Size(100, 100);
                            tile.DetermineTilePicture();

                            tile.Enabled = false;

                            PlayingBoard[x, y] = tile;

                        }
                        else
                        {
                            Tile tile = new Tile((y * PlayingBoard.GetLength(0)) + (x + 1), random);
                            tile.Size = new Size(100, 100);

                            tile.DetermineTilePicture();
                            
                            PlayingBoard[x, y] = tile;
                        }
                    }
                }

                FreeTile.Size = new Size(100, 100);

                FreeTile.DetermineTilePicture();
            }
            /// <summary>
            /// Places the Tile objects from the PlayingBoard matrix
            /// </summary>
            /// <param name="c">Which control the objects should be added</param>
            public void PlaceTiles(Control c)
            {
                int y = 10, x = 10;
                for (int i = 0; i < PlayingBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < PlayingBoard.GetLength(1); j++)
                    {

                        if (PlayingBoard[i, j] != null)
                        {
                            c.Controls.Add(PlayingBoard[i, j]);
                            PlayingBoard[i, j].Location = new Point(y, x);
                            x += 110;
                        }
                        else
                        {
                            x += 110;
                        }
                    }
                    x = 10;
                    y += 110;
                }
                FreeTile.Location = new Point(1100, 10);
                c.Controls.Add(FreeTile);
            }           
        }
        //------------------------------
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
