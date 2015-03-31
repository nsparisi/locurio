using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    class Program
    {
        static void Main(string[] args)
        {
            AbyssRunner runner = new AbyssRunner();
            runner.Start();

            Thread.Sleep(30000);

            Console.Write("End Program");
            Environment.Exit(0);
        }
    }
}
