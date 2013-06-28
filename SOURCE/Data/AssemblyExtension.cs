//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtension.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary> </summary>
    public static class AssemblyExtension
    {
        /// <summary> </summary>
        public static string GetCurrentExecutingDirectory(this Assembly assembly)
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }
}
