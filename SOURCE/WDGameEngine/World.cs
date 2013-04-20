//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// <inheritDoc/>
    /// </summary>
    public class World : IWorld
    {
        /// <summary>
        /// An IWDRepository instance.
        /// </summary>
        private IWDRepository wdRepository;

        /// <summary>
        /// Initializes a World instance.
        /// </summary>
        /// <param name="wdRepository">An IWDRepository instance. </param>
        public World(IWDRepository wdRepository)
        {
            this.wdRepository = wdRepository;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public List<Continent> Continents { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public List<Country> Countries { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public void LoadContinentsAndCountries()
        {
            this.Continents = this.wdRepository.GetContinents();
            this.Countries = this.Continents.SelectMany(c => c.Countries).ToList();
            List<Neighbours> neighbours = this.wdRepository.GetNeighbours();
            foreach (Neighbours n in neighbours)
            {
                Country country1 = this.Countries.Find(c => c.Name == n.Country1);
                Country country2 = this.Countries.Find(c => c.Name == n.Country2);
                country1.Neighbours.Add(country2);
                country2.Neighbours.Add(country1);
            }
        }
    }
}
