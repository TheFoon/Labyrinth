using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
        private LobbyHandler lobbyHandler = new LobbyHandler();

        public GameHandler() {

        }

        public void Start() {

        }

        //Start hosting game
        public void HostGame() {
            lobbyHandler.HostBroadcast(true);
            playerStatus = PlayerStatus.Host;
        }

        public void JoinGame() {
            lobbyHandler.RecieveHosts(true);
            playerStatus = PlayerStatus.Joining;
        }
    }

    class BoardHandler
    {

    }

    class LobbyHandler
    {
        private IPAddress IP_ADDRESS;
        private IPAddress IP_BROADCAST;

        public LobbyHandler() {
            string info = GetIP();
            IP_ADDRESS = IPAddress.Parse(info.Split(' ')[0]);
            IPAddress mask = IPAddress.Parse(info.Split(' ')[1]);
            IP_BROADCAST = GetBroadcastAddress(IP_ADDRESS, mask);
        }

        public void HostBroadcast(bool a) {

        }

        public void RecieveHosts(bool a)
        {

        }

        private string GetIP()
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

        public IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
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
