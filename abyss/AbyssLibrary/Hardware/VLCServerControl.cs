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
    public class VLCServerControl : AbstractPhysicalObject
    {
        

        const string urlBase = "http://localhost:8080/requests/status.xml";
        const string soundfile = @"08 Breaking Out.mp3";
        
        public VLCServerControl()
            : base()
        {
            //Test();
        }

        public VLCServerControl(string name)
            : base(name)
        {
            //Test();
        }

        void Test()
        {
            Play(soundfile);
            Pause();
            Pause();
            SetVolume(50);
        }

        public void Play(string fileName)
        {
            const string clearPlaylist = "?command=pl_empty";
            ExecuteWebRequest(clearPlaylist);

            const string playSong = "?command=in_play&input=";
            string songPath = Path.Combine(AbyssUtils.AbyssSoundDirectory, fileName);
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

        void ExecuteWebRequest(string command)
        {
            HttpWebResponse response = null;
            Stream receiveStream = null;
            try
            {
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
            catch(Exception e)
            {
                Debug.Log("Ran into an error during ExecuteWebRequest {0}", e.InnerException);
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
}
