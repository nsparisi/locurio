﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AbyssLibrary
{
    public class AbstractNetworkedDevice : AbstractPhysicalObject, INetworkedDevice
    {
        private string macAddress;
        private string ipAddress;

        private bool isRefreshing = false;
        private object lockObj = new object();

        public AbstractNetworkedDevice(string name, string macAddress, string bestGuessIpAddress)
            : base(name)
        {
            this.macAddress = macAddress;
            this.BestGuessIpAddress = bestGuessIpAddress;
            RefreshIpAddress(true);
        }

        public string MacAddress
        {
            get 
            { 
                return this.macAddress;
            }
        }

        public string IpAddress
        {
            get 
            { 
                return this.ipAddress;
            }
        }

        public bool IsConnected
        {
            get 
            {
                return !string.IsNullOrEmpty(this.ipAddress);
            }
        }

        public string BestGuessIpAddress { get; set; }

        public void RefreshIpAddress(bool scanNetworkIfNotPresent = false)
        {
            if (!isRefreshing)
            {
                Thread t;
                
                if(scanNetworkIfNotPresent)
                {
                    t = new Thread(ThreadCheckAndRefreshIpAddress);
                }
                else
                {
                    t = new Thread(ThreadCheckIpAddress);
                }

                t.IsBackground = true;
                t.Start();
            }
        }

        private void ThreadCheckIpAddress()
        {
            lock (lockObj)
            {
                isRefreshing = true;
                ipAddress = NetworkHelper.Instance.GetIpAddress(macAddress, false, BestGuessIpAddress);
                isRefreshing = false;

                LogConnectionStatus();
            }
        }

        private void ThreadCheckAndRefreshIpAddress()
        {
            for (int i = 0; i < 4; i++)
            {
                lock (lockObj)
                {
                    isRefreshing = true;
                    ipAddress = NetworkHelper.Instance.GetIpAddress(macAddress, true, BestGuessIpAddress);
                    isRefreshing = false;

                    LogConnectionStatus();

                    if (IsConnected)
                    {
                        break;
                    }

                }
            }
        }

        private void LogConnectionStatus()
        {
            if (IsConnected)
            {
                Debug.Log("Connected to {0} ({1}) at {2}", Name, macAddress, ipAddress);
            }
            else
            {
                Debug.Log("Failed to connect to {0} ({1})", Name, macAddress);
            }
        }
    }
}
