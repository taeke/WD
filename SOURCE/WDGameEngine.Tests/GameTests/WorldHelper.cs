//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldHelper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    //// How the world looks like for the tests.
    //// _______________________________________________________________
    //// | Continent 0                  | Continent 1                  |
    //// |  ____________________________|  ____________________________|
    //// |  | Country 0   | Country 1   |  | Country 2   | Country 3   |
    //// |  | Red player  | Red player  |  | Red player  | Red player  |
    //// |  |_____________|_____________|  |_____________|_____________|
    //// |  | Country  4  | Country 5   |  | Country 6   | Country 7   |
    //// |  | Red player  | Red player  |  | Red player  | Red player  |
    //// |__|_____________|_____________|__|_____________|_____________|
    //// | Continent 2                  | Continent 3                  |
    //// |  ____________________________|  ____________________________|
    //// |  | Country 8   | Country 9   |  | Country 10  | Country 11  |
    //// |  | Blue player | Blue player |  | Blue player | Blue player |
    //// |  |_____________|_____________|  |_____________|_____________|
    //// |  | Country 12  | Country 13  |  | Country 14  | Country 15  |
    //// |  | Blue player | Blue player |  | Blue player | Blue player |
    //// |__|_____________|_____________|__|_____________|_____________|
    
    using System.Collections.Generic;
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// We need a Mock of <see cref="IWorld"/> for a lot of the tests. This class is a wrapper for this Mock
    /// so we have one place for the variables we use for the setup of this Mock.
    /// </summary>
    public class WorldHelper
    {
        /// <summary>
        /// A collection of <see cref="Country"/> for the <see cref="IWorld"/> mock.
        /// </summary>
        private List<Country> countries = new List<Country>();

        /// <summary>
        /// A collection of <see cref="Continent"/> for the <see cref="IWorld"/> mock.
        /// </summary>
        private List<Continent> continents = new List<Continent>();

        /// <summary>
        /// Backing field voor WorldMock.
        /// </summary>
        private Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);

        /// <summary>
        /// Creates an instance of the Helper class and creates the setup for the mock.
        /// </summary>
        public WorldHelper()
        {
            this.CreateCountriesForMock();
            this.CreateContinentsForMock();
            this.worldMock.Setup(w => w.Countries).Returns(this.countries);
            this.worldMock.Setup(w => w.Continents).Returns(this.continents);
            this.worldMock.Setup(w => w.LoadContinentsAndCountries());
        }

        /// <summary>
        /// The mock for <see cref="IWorld"/>.
        /// </summary>
        public Mock<IWorld> WorldMock
        {
            get
            {
                return this.worldMock;
            }
        }

        /// <summary>
        /// The mocked <see cref="IWorld"/> object.
        /// </summary>
        public IWorld World
        {
            get
            {
                return this.worldMock.Object;
            }
        }

        /// <summary>
        /// Creates a set of <see cref="Country"/> which can be used for testing. Its a grid of 4 by 4
        /// <see cref="Country"/>. 
        /// </summary>
        private void CreateCountriesForMock()
        {
            this.countries = new List<Country>();
            for (int i = 0; i < 16; i++)
            {
                this.countries.Add(new Country() { Name = i.ToString() });
            }

            for (int i = 0; i < 16; i++)
            {
                // left of current. 
                if ((i + 1) % 4 != 1)
                {
                    this.countries[i].Neighbours.Add(this.countries[i - 1]);
                }

                // right of current.
                if ((i + 1) % 4 != 0)
                {
                    this.countries[i].Neighbours.Add(this.countries[i + 1]);
                }

                // top of current
                if ((i + 1) > 4)
                {
                    this.countries[i].Neighbours.Add(this.countries[i - 4]);
                }

                // bottom of current
                if ((i + 1) < 12)
                {
                    this.countries[i].Neighbours.Add(this.countries[i + 4]);
                }
            }
        }

        /// <summary>
        /// continents is for the test a 2 by 2 grid of <see cref="Continent"/> each containing a 2 by 2 grid of <see cref="Country"/>
        /// </summary>
        private void CreateContinentsForMock()
        {
            for (int i = 0; i < 4; i++)
            {
                this.continents.Add(new Continent() { NewArmiesForOwning = (i + 1) * 2 });
                this.continents[i].Countries.Add(this.countries[(i < 2 ? 0 : 4) + (i * 2)]);
                this.continents[i].Countries.Add(this.countries[(i < 2 ? 0 : 4) + (i * 2) + 1]);
                this.continents[i].Countries.Add(this.countries[(i < 2 ? 0 : 4) + (i * 2) + 4]);
                this.continents[i].Countries.Add(this.countries[(i < 2 ? 0 : 4) + (i * 2) + 5]);
            }
        }
    }
}
