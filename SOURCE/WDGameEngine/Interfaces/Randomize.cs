//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Randomize.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    /// <summary>
    /// There is not interface for the default Random function but for testing it can be handy
    /// to generate not ramdom numbers. Use an abstract class so we can Mock it.
    /// </summary>
    public abstract class Randomize
    {
        /// <summary>
        /// The method we need to Mock
        /// </summary>
        /// <param name="maxValue"> The maximum value Next may return. </param>
        /// <returns> A number which will be random when the Random function is used. </returns>
        public abstract int Next(int maxValue);
    }
}
