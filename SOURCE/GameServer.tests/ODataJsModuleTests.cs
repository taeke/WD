namespace GameServer.Tests
{
    using System.Collections.Generic;
    using Data.Model;
    using GameServer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nancy;
    using Nancy.Testing;

    [TestClass]
    public class ODataJsModuleTests
    {
        [TestMethod]
        public void ShouldReturnODatajs()
        {
            // Arrange
            var bootstrapper = new DefaultNancyBootstrapper();
            var visualCountryRepository = new Mock<IVisualCountryRepository>(MockBehavior.Strict);
            visualCountryRepository.Setup(v => v.GetAllVisualCountries()).Returns(
                new List<VisualCountry>
                {
                    new VisualCountry 
                    { 
                        Name = "Alaska",
                        BorderPoints = new List<MapPoint>
                        {
                            new MapPoint 
                            {
                                X = 342,
                                Y = 238
                            },
                            new MapPoint
                            {
                                X = 301,
                                Y = 227
                            },
                            new MapPoint
                            {
                                X = 266,
                                Y = 207
                            }
                        },
                        NumberCenterPoint = new MapPoint
                        {
                            X = 265,
                            Y = 296
                        },
                        NumberEndPoint = new MapPoint
                        {
                            X = 265,
                            Y = 296
                        }
                    }
                });
            var browser = new Browser((c) => c.Module<ODataJsModule>().Dependency<IVisualCountryRepository>(visualCountryRepository.Object));

            // Act
            var result = browser.Get(
                "/content/oData.js",
                with =>
                {
                    with.HttpRequest();
                });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
