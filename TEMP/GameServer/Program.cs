namespace GameServer
{
    using System;
    using Microsoft.Owin.Hosting;
    using Data;

    /// <summary>
    /// The communication server between all other exe's in this project. Communication is handled by SignalR.
    /// It also hosts the HTML en javascript for viewing the game progress visualy.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Start the Console App.
        /// </summary>
        /// <param name="args"> Parameters. </param>
        public static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApplication.Start<Startup>(url))
            {
                Console.ReadLine();
            }
        }
    }
}
