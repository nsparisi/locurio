using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    class NetworkHelper
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(
            uint DestIP, uint SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

        private static object scanLock = new Object();
        private static object propertyLock = new Object();

        Dictionary<string, string> macToIpTable;
        bool isInitialized;

        public bool IsScanning { get; private set; }

        private static volatile NetworkHelper instance;
        public static NetworkHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (propertyLock)
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
            isInitialized = false;
        }

        public void ClearAndRefreshCache()
        {
            if(IsScanning)
            {
                return;
            }

            lock (scanLock)
            {
                Debug.Log("Scanning network for IPs... ");
                IsScanning = true;

                macToIpTable.Clear();

                string gateway = GetDefaultGatewayOrBestGuess();
                string baseip = gateway.Substring(0, gateway.LastIndexOf('.') + 1);
                List<Thread> threads = new List<Thread>();
                for (int i = 1; i <= 254; i++)
                {
                    Thread t = new Thread(SendArp);
                    t.Start(baseip + i.ToString());
                    threads.Add(t);
                }

                foreach(Thread t in threads)
                {
                    t.Join();
                }

                IsScanning = false;
                isInitialized = true;
                Debug.Log("Finished IP scan. ");
            }
        }

        public string GetIpAddress(string macAddress, bool scanNetworkIfNotPresent = false)
        {
            //Debug.Log("Getting IP address for MAC: {0}", macAddress);
            if(string.IsNullOrEmpty(macAddress))
            {
                return string.Empty;
            }

            // if the IP is not in the cache
            // determine if we should scan again
            bool shouldScanNetwork;
            lock (propertyLock)
            {
                shouldScanNetwork = (!isInitialized || scanNetworkIfNotPresent) && !IsScanning;
                isInitialized = true;
            }

            // check the cache for this entry
            lock (scanLock)
            {
                macAddress = macAddress.ToUpper();
                if (macToIpTable.ContainsKey(macAddress))
                {
                    Debug.Log("Found MAC {0} mapped to IP {1}.", macAddress, macToIpTable[macAddress]);
                    return macToIpTable[macAddress];
                }

                // no entry found in cache
                // if we explicitly want to refresh the cache, and we're not already refreshing it
                // then refresh the cache and try again
                if (!shouldScanNetwork)
                {
                    Debug.Log("Could not find MAC {0} on the network.", macAddress);
                    return string.Empty;
                }

                ClearAndRefreshCache();

                if (macToIpTable.ContainsKey(macAddress))
                {
                    Debug.Log("Found MAC {0} mapped to IP {1}.", macAddress, macToIpTable[macAddress]);
                    return macToIpTable[macAddress];
                }
            }

            Debug.Log("Could not find MAC {0} on the network.", macAddress);
            return string.Empty;
        }

        /// <summary>
        /// Send ARP function used for thread objects.
        /// </summary>
        /// <param name="ipAddress"></param>
        private void SendArp(object ipAddress)
        {
            if(ipAddress != null && ipAddress is string)
            {
                SendArp((string)ipAddress);
            }
        }

        /// <summary>
        /// Returns an uppercase, colon-delimited MAC address or 
        /// an empty string if ARP request was unsuccessful.
        /// </summary>
        /// <param name="ipAddress">IP address of the destination</param>
        /// <returns>MAC address i.e. AA:BB:CC:DD:EE:FF</returns>
        private void SendArp(string ipAddress)
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
                    if (retValue != 67)
                    {
                        Debug.Log("ARP request for IP: {0} failed with result {1}.", ipAddress, retValue);
                    }

                    return;
                }

                // convert the mac bytes into a string array
                string[] str = new string[macAddressLength];
                for (int i = 0; i < macAddressLength; i++)
                {
                    str[i] = macAddress[i].ToString("x2");
                }

                // convert the string array into a single string
                string resultantMac = string.Join(":", str).ToUpper();
                string resultantMac2 = string.Join("-", str).ToUpper();

                lock (propertyLock)
                {
                    // add the ip-mac mapping to our cache
                    if (!string.IsNullOrEmpty(resultantMac) && !macToIpTable.ContainsKey(resultantMac))
                    {
                        macToIpTable.Add(resultantMac, ipAddress);
                    }
                    if (!string.IsNullOrEmpty(resultantMac2) && !macToIpTable.ContainsKey(resultantMac2))
                    {
                        Debug.Log("Matched MAC: '{0}'to IP: {1}", resultantMac2, ipAddress);
                        macToIpTable.Add(resultantMac2, ipAddress);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.Log("There was an error creating the ARP request for IP: {0}", ipAddress);
                Debug.Log("Error: ", e);
            }
        }

        private string GetDefaultGatewayOrBestGuess()
        {
            string defaultGateway = "192.168.0.1";
            try
            {
                foreach (var iface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var address = iface.GetIPProperties().GatewayAddresses.FirstOrDefault(
                        ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    if (address != null)
                    {
                        defaultGateway = address.Address.ToString();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("There was an error getting the default gateway. Make sure we're connected to the network.", e);
            }

            return defaultGateway;
        }

        private void SendPing(string ipAddress)
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
