//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldTests.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// The tests for the World class.
    /// </summary>
    [TestClass]
    public class WorldTests
    {
        /// <summary>
        /// The instance of the class under test.
        /// </summary>
        private World world;

        private Mock<IWDRepository> wdRepositoryMock = new Mock<IWDRepository>(MockBehavior.Strict);

        /// <summary>
        /// Initializing for every test.
        /// </summary>
        [TestInitialize]
        public void PlayerInitialize()
        {
            this.wdRepositoryMock.Setup(w => w.GetContinents()).Returns(new List<Continent> 
            {
                new Continent
                {
                    Name = "0",
                    Countries = new List<Country> { new Country { Name = "0" }, new Country { Name = "1" }, new Country { Name = "4" }, new Country { Name = "5" } }
                },
                new Continent
                {
                    Name = "1",
                    Countries = new List<Country> { new Country { Name = "2" }, new Country { Name = "3" }, new Country { Name = "6" }, new Country { Name = "7" } }
                }
            });

            this.wdRepositoryMock.Setup(w => w.GetNeighbours()).Returns(new List<Neighbours>
            {
                new Neighbours { Country1 = "0", Country2 = "1" },
                new Neighbours { Country1 = "0", Country2 = "4" },
                new Neighbours { Country1 = "1", Country2 = "2" },
                new Neighbours { Country1 = "1", Country2 = "5" },
                new Neighbours { Country1 = "2", Country2 = "3" },
                new Neighbours { Country1 = "2", Country2 = "6" },
                new Neighbours { Country1 = "3", Country2 = "7" },
                new Neighbours { Country1 = "4", Country2 = "5" },
                new Neighbours { Country1 = "5", Country2 = "6" },
                new Neighbours { Country1 = "6", Country2 = "7" }
            });

            this.world = new World(this.wdRepositoryMock.Object);
        }

        /// <summary>
        /// The tests for the LoadContinentsAndCountries Method.
        /// </summary>
        [TestClass]
        public class TheLoadContinentsAndCountriesMethod : WorldTests
        {
            /// <summary>
            /// Testing if the Continents en Countries get initialized the right way.
            /// </summary>
            [TestMethod]
            public void ShouldHaveInitialisedWorld()
            {
                // arange

                // act
                this.world.LoadContinentsAndCountries();

                // assert
                Assert.AreEqual(2, this.world.Continents.Count);
                Assert.AreEqual(8, this.world.Countries.Count);
                Country country0 = this.world.Countries.Find(c => c.Name == "0");
                Country country1 = this.world.Countries.Find(c => c.Name == "1");
                Continent continent = this.world.Continents.Find(c => c.Name == "0");
                Assert.AreEqual(country0, continent.Countries.Find(c => c.Name == "0"));
                Assert.AreEqual(country1, country0.Neighbours.Find(c => c.Name == "1"));
            }
        }
    }
}
