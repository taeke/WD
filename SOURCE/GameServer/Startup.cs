//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace GameServer
{
    using Microsoft.AspNet.SignalR;
    using Owin;

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration();
            app.MapHubs(config);
            app.UseNancy();
        }
    }
}
