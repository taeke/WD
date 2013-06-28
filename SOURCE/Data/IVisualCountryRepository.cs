//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IVisualCountryRepository.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace GameServer
{
    using System.Collections.Generic;
    using Data.Model;

    public interface IVisualCountryRepository
    {
        List<VisualCountry> GetAllVisualCountries();
    }
}
