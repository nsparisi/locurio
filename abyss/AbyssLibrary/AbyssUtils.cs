using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class AbyssUtils
    {
        public static string AbyssCommonDirectory
        {
            get
            {
                
                return Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    "Abyss");
            }
        }

        public static string AbyssSoundDirectory
        {
            get
            {
                return Path.Combine(AbyssCommonDirectory, "Sounds");
            }
        }
    }
}
