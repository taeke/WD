//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseTests.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace CreateDB.Tests
{
    using CreateDB;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Database class.
    /// </summary>
    [TestClass]
    public class DatabaseTests
    {
        /// <summary>
        /// The instance of the class under test.
        /// </summary>
        private DataBase dataBase;

        /// <summary>
        /// Initializing for every test.
        /// </summary>
        [TestInitialize]
        public void DataBaseInitialize()
        {
            this.dataBase = new DataBase();
        }

        /// <summary>
        /// Tests for the Execute method.
        /// </summary>
        [TestClass]
        public class TheExecuteMethod : DatabaseTests
        {
            /// <summary>
            /// Not a real test. Just it tric for debugging the Build Task.
            /// </summary>
            [TestMethod]
            public void ShouldCreateDB()
            {
                this.dataBase.DataBaseFile = "WD.sqlite";
                this.dataBase.QueryFile = @"..\..\..\Data\QueryStrings.txt";
                this.dataBase.Execute();
            }
        }
    }
}
