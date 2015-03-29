using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    class Debug
    {
        public static void Log(string message, params object[] parameters)
        {
            Console.Out.WriteLine(string.Format(message, parameters));
        }

        public static void Log(object objectToPrint)
        {
            Console.Out.WriteLine(objectToPrint);
        }
    }
}
