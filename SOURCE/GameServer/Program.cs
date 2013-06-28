//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace GameServer
{
    using System;
    using Microsoft.Owin.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine(@"http://localhost:8080/content/playfield.html");
                Console.ReadLine();
            }
        }
    }
}
