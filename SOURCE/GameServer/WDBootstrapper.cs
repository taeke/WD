//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="WDBootstrapper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace GameServer
{
    using Data;
    using Nancy;
    using Nancy.TinyIoc;

    /// <summary>
    /// We need our own bootstrapper to inject the VisualCountryRepository.
    /// </summary>
    public class WDBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<IVisualCountryRepository, VisualCountryRepository>().AsSingleton();
        }
    }
}
