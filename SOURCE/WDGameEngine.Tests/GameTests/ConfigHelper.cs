//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigHelper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System.Collections.Generic;
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// We need a mock of <see cref="IConfig"/> for a lot of the tests. This class is a wrapper for this Mock.
    /// So we have one place for the variables we use for the setup of this Mock. 
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// The mock for <see cref="IConfig"/>.
        /// </summary>
        private Config config = new Config();

        /// <summary>
        /// Creates an instance of the Helper class and creates the setup for the mock.
        /// </summary>
        public ConfigHelper()
        {
            Dictionary<int, int> numberOfPlayersIntialArmies = new Dictionary<int, int>();
            numberOfPlayersIntialArmies.Add(2, 40);
            this.config.MinimumNumberPlayers = 2;
            this.config.NumberOfPlayersIntialArmies = numberOfPlayersIntialArmies;
            this.config.MaximumCards = 5;
        }

        /// <summary>
        /// The mocked <see cref="IWorld"/> object.
        /// </summary>
        public Config Config
        {
            get
            {
                return this.config;
            }
        }
    }
}
