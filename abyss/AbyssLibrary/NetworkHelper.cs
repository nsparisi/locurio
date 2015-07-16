using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    class NetworkHelper
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(
            uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

        private static object lockObj = new Object();

        Dictionary<string, string> macToIpTable;

        private static volatile NetworkHelper instance;
        public static NetworkHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new NetworkHelper();
                        }
                    }
                }

                return instance;
            }
        }

        private NetworkHelper()
        {
            macToIpTable = new Dictionary<string, string>();
            ClearAndRefreshCache();
        }

        public void ClearAndRefreshCache()
        {
            lock (lockObj)
            {
                macToIpTable.Clear();
            }

            string baseip = "192.168.1.";
            string[] ipAddresses = new string[255];
            for(int i = 1; i <= 255; i++)
            {
                ipAddresses[i-1] = baseip + i.ToString();

                SendArp(baseip + i.ToString());
            }
        }

        public string GetIpAddress(string macAddress)
        {
            Debug.Log("Getting IP address for MAC: {0}", macAddress);
            if(string.IsNullOrEmpty(macAddress))
            {
                return string.Empty;
            }

            macAddress = macAddress.ToUpper();
            if(macToIpTable.ContainsKey(macAddress))
            {
                return macToIpTable[macAddress];
            }

            Debug.Log("Could not find MAC {0} on the network.", macAddress);
            return string.Empty;
        }

        /// <summary>
        /// Returns an uppercase, colon-delimited MAC address or 
        /// an empty string if ARP request was unsuccessful.
        /// </summary>
        /// <param name="ipAddress">IP address of the destination</param>
        /// <returns>MAC address i.e. AA:BB:CC:DD:EE:FF</returns>
        public void SendArp(string ipAddress)
        {
            try
            {
                // run the ARP request for the IP destination
                IPAddress destinationAddress = IPAddress.Parse(ipAddress);
                uint uintAddress = BitConverter.ToUInt32(destinationAddress.GetAddressBytes(), 0);
                byte[] macAddress = new byte[6];
                int macAddressLength = macAddress.Length;
                int retValue = SendARP(uintAddress, 0, macAddress, ref macAddressLength);
                if (retValue != 0)
                {
                    throw new Win32Exception(retValue, "SendARP failed.");
                }

                // convert the mac bytes into a string array
                string[] str = new string[macAddressLength];
                for (int i = 0; i < macAddressLength; i++)
                {
                    str[i] = macAddress[i].ToString("x2");
                }

                // convert the string array into a single string
                string resultantMac = string.Join(":", str).ToUpper();

                lock (lockObj)
                {
                    // add the ip-mac mapping to our cache
                    if (!string.IsNullOrEmpty(resultantMac) && !macToIpTable.ContainsKey(resultantMac))
                    {
                        Debug.Log("Finished arp request with MAC: '{0}', IP: {1}", resultantMac, ipAddress);
                        macToIpTable.Add(resultantMac, ipAddress);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.Log("There was an error creating the ARP request for IP: {0}", ipAddress);
                Debug.Log("Error: ", e);
            }
        }

        public void SendPing(string ipAddress)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions pingOptions = new PingOptions();

                pingOptions.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;

                PingReply pingReply = pingSender.Send(ipAddress, timeout, buffer, pingOptions);

                if (pingReply.Status != IPStatus.Success)
                {
                    Debug.Log("FAILURE. Could not retrieve reply from destination {0} with status {1}",
                        ipAddress,
                        pingReply.Status);
                }
                else
                {
                    Debug.Log("SUCCESS. ping destination {0}", ipAddress);
                }
            }
            catch (Exception e)
            {
                Debug.Log("There was an error with the PING request", e);
            }
        }
    }
}
