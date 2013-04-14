//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomHelper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Helper class for random numbers so we can control the numbers during testing.
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// Mock for the <see cref="Randomize"/>.
        /// </summary>
        private Mock<Randomize> randomMock = new Mock<Randomize>();

        /// <summary>
        /// Is <see cref="Game"/> using the <see cref="Randomize"/> class for Dividing the <see cref="Country"/> ?
        /// </summary>
        private bool isDivideCountries = false;

        /// <summary>
        /// Creates an instance of the Helper class and creates the setup for the mock.
        /// </summary>
        public RandomHelper()
        {
            this.randomMock.Setup(r => r.Next(It.IsAny<int>())).Returns<int>(maxValue => this.RandomNext(maxValue));
        }

        /// <summary>
        /// The mocked <see cref="Randomize"/> object.
        /// </summary>
        public Randomize Random
        {
            get
            {
                return this.randomMock.Object;
            }
        }

        /// <summary>
        /// Generate pseudo ramdom numbers for the tests. We know countries will be divide by starting to
        /// ask for a random number between 15 and 0. The last call for dividing the <see cref="Country"/>
        /// will be 0. So that is the moment to reset this bool. This way we control the way the <see cref="Country"/>
        /// collections is divide.
        /// </summary>
        /// <param name="maxValue"> The maximum value which kan be returned. </param>
        /// <returns> A pseudo random number. </returns>
        private int RandomNext(int maxValue)
        {
            if (maxValue == 15)
            {
                this.isDivideCountries = true;
            }

            if (this.isDivideCountries)
            {
                if (maxValue % 2 != 0)
                {
                    return (int)maxValue / 2;
                }
                else
                {
                    return ((int)maxValue / 2) + ((int)maxValue / 2);
                }
            }

            if (maxValue == 0)
            {
                this.isDivideCountries = false;
            }

            return 1;
        }
    }
}
