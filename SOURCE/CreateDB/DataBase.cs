//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBase.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace CreateDB
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary> Een build task voor het aanmaken van de database. />. </summary>
    public class DataBase : Task
    {
        /// <summary> De location of de database file. </summary>
        [Required]
        public string DataBaseFile { get; set; }

        [Required]
        public string QueryFile { get; set; }

        /// <summary> Creates the database. </summary>
        public override bool Execute()
        {
            this.CreateDatabaseFile();
            this.ExecuteNonQueryTransactionally(this.GetQueryStrings());
            return true;
        }

        /// <summary>
        /// Creates the file on disk.
        /// </summary>
        private void CreateDatabaseFile()
        {
            SQLiteConnection.CreateFile(this.DataBaseFile);
        }

        /// <summary>
        /// Get the queries for creating and filling de tables for the game.
        /// </summary>
        /// <returns> A list of query strings</returns>
        private List<string> GetQueryStrings()
        {
            using (StreamReader streamReader = new StreamReader(this.QueryFile))
            {
                string text = streamReader.ReadToEnd();
                return text.Split(';').ToList();
            }
        }

        /// <summary>
        /// Fires all the queries provided by the list of strings add the database file in one transaction.
        /// </summary>
        /// <param name="queryList"></param>
        private void ExecuteNonQueryTransactionally(List<string> queryList)
        {
            using (var sqliteConnection = new SQLiteConnection("Data Source=" + this.DataBaseFile))
            {
                sqliteConnection.Open();
                using (var trans = sqliteConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (string s in queryList)
                        {
                            using (var cmd = new SQLiteCommand(sqliteConnection))
                            {
                                cmd.CommandText = s;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                    }
                    catch (SQLiteException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
