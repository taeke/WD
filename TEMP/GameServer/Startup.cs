namespace GameServer
{
    using Microsoft.AspNet.SignalR;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration();
            app.MapHubs(config);
            app.UseNancy();
        }
    }
}
