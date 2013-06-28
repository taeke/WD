namespace GameServer
{
    using Data;
    using Nancy;
    using Nancy.TinyIoc;

    public class WDBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<IVisualCountryRepository, VisualCountryRepository>().AsSingleton();
        }
    }
}
