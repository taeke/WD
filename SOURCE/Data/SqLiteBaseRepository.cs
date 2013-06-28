//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="SqLiteBaseRepository.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Data.SQLite;
    using System.IO;
    using System.Reflection;

    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + @"\WD.sqlite"; }
        }

        public static SQLiteConnection SQLiteDBConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}
