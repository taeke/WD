//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualCountryRepository.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using Data.Model;
    using GameServer;

    public class VisualCountryRepository : SqLiteBaseRepository, IVisualCountryRepository
    {
        private const string MapPointQuery = @"SELECT ID, X, Y FROM MapPoint";

        private const string MapPointCountriesQuery = @"SELECT IDMapPoint, IDCountry, Volgnummer FROM MapPointCountry";

        private const string VisualCountriesQuery = @"SELECT c.ID, c.Name, pc.ID, pc.X, pc.Y, pe.ID, pe.X, pe.Y
                                                      FROM Country c
                                                      INNER JOIN MapPoint pc ON pc.ID = c.IDNumberCenterPoint
                                                      INNER JOIN MapPoint pe ON pe.ID = c.IDNumberENDPoint ";

        private List<VisualCountry> visualCountries;
        
        private List<MapPoint> mapPoints;

        private List<MapPointCountry> mapPointCountries;

        public List<VisualCountry> GetAllVisualCountries()
        {
            using (var sQLiteDBConnection = SQLiteDBConnection())
            {
                sQLiteDBConnection.Open();
                this.mapPoints = sQLiteDBConnection.Query<MapPoint>(MapPointQuery).ToList();
                this.mapPointCountries = sQLiteDBConnection.Query<MapPointCountry>(MapPointCountriesQuery).OrderBy(m => m.Volgnummer).ToList();
                this.visualCountries = sQLiteDBConnection.Query<VisualCountry, MapPoint, MapPoint, VisualCountry>(
                    VisualCountriesQuery,
                    (c, pc, pe) => { c.NumberCenterPoint = pc; c.NumberEndPoint = pe; return c; }).ToList();
                this.GetBorderPointsForVisualCountries();
            }

            return this.visualCountries;
        }

        private void GetBorderPointsForVisualCountries()
        {
            foreach (var visualCountry in this.visualCountries)
            {
                visualCountry.BorderPoints = new List<MapPoint>();
                foreach (var mapPointCountry in this.mapPointCountries)
                {
                    if (mapPointCountry.IDCountry == visualCountry.ID)
                    {
                        visualCountry.BorderPoints.Add(this.mapPoints.Find(p => p.ID == mapPointCountry.IDMapPoint));
                    }
                }
            }
        }
    }
}
