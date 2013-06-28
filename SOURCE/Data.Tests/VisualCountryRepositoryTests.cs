//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualCountryRepositoryTests.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace Data.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VisualCountryRepositoryTests
    {
        /// <summary>
        /// The instance of the class under test.
        /// </summary>
        private VisualCountryRepository visualCountryRepository;

        /// <summary>
        /// Initializing for every test.
        /// </summary>
        [TestInitialize]
        public void VisualCountryRepositoryInitialize()
        {
            this.visualCountryRepository = new VisualCountryRepository();
        }

        [TestClass]
        public class TheGetAllVisualCountriesMethod : VisualCountryRepositoryTests
        {
            [TestMethod]
            public void ShouldReturnCountries()
            {
                // Arrange

                // Act
                var visualCountries = visualCountryRepository.GetAllVisualCountries();

                // Assert
                Assert.IsNotNull(visualCountries);
            }
        }
    }
}
