using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AbyssLibrary
{
    public class VLCServerControl : AbstractNetworkedDevice
    {
        public OSType DeviceOS;
        public string DevicePort;

        public enum OSType { Linux, Windows }

        const string RequestURLFormat = "http://{0}:{1}/requests/status.xml";

        private object lockObj = new object();

        public string ServerURL
        {
            get
            {
                if (IsConnected)
                {
                    return string.Format("http://{0}:{1}", IpAddress, DevicePort);
                }

                return string.Empty;
            }
        }

        string AudioDirectory
        {
            get
            {
                string directory = "";
                if(this.DeviceOS == OSType.Linux)
                {
                    directory = "/home/pi/";
                }
                else if(this.DeviceOS == OSType.Windows)
                {
                    directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                return this.PathCombineOSSpecific(directory, "Abyss", "Sounds");
            }
        }

        public VLCServerControl(string name, string deviceMacAddress, OSType deviceOS, string devicePort)
            : base(name, deviceMacAddress)
        {
            this.DeviceOS = deviceOS;
            this.DevicePort = devicePort;
        }

        public void Play(string fileName)
        {
            string clearPlaylist = "?command=pl_empty";
            ExecuteWebRequest(clearPlaylist);

            const string playSong = "?command=in_play&input=";
            string songPath = this.PathCombineOSSpecific(AudioDirectory, fileName);
            ExecuteWebRequest(playSong + songPath);
        }

        public void Pause()
        {
            string pauseSong = "?command=pl_pause&id=0";
            ExecuteWebRequest(pauseSong);
        }

        public void SetVolume(float volume)
        {
            string setVolume = "?command=volume&val=" + volume;
            ExecuteWebRequest(setVolume);
        }

        public void Stop()
        {
            string clearPlaylist = "?command=pl_empty";
            ExecuteWebRequest(clearPlaylist);
        }

        void ExecuteWebRequest(string command)
        {
            if(!this.IsConnected)
            {
                Debug.Log("Cannot execute web request. VLC server controller '{0}' is not connected to the VLC server at '{1}'.", this.Name, this.MacAddress);
                return;
            }

            lock (lockObj)
            {
                HttpWebResponse response = null;
                Stream receiveStream = null;
                try
                {
                    string urlBase = string.Format(RequestURLFormat, this.IpAddress, this.DevicePort);
                    HttpWebRequest request = WebRequest.CreateHttp(urlBase + command);
                    request.Proxy = WebRequest.DefaultWebProxy;
                    response = (HttpWebResponse)request.GetResponse();

                    XmlDocument xmlDoc = new XmlDocument();

                    if (response != null)
                    {
                        receiveStream = response.GetResponseStream();
                        xmlDoc.Load(receiveStream);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Ran into an error during ExecuteWebRequest for VLC server.", e);
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }

                    if (receiveStream != null)
                    {
                        receiveStream.Close();
                    }
                }
            }
        }

        string PathCombineOSSpecific(params string[] paths)
        {
            string combinedPath = Path.Combine(paths);

            if (this.DeviceOS == OSType.Linux)
            {
                combinedPath = combinedPath.Replace('\\', '/');
            }

            return combinedPath;
        }
    }
}
