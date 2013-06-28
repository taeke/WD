//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeVisualCountryHub.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace SignalRTest5
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    /// <summary>
    /// Send a change in color and/or number of armies for a country to the client showing the game.
    /// </summary>
    [HubName("changeVisualCountryHub")]
    public class ChangeVisualCountryHub : Hub
    {
        public void GetChangeCountry(string country, string color, string number)
        {
            Clients.All.setChangeCountry(country, color, number);
        }
    }
}
