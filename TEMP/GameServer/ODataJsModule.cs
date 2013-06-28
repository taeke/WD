namespace GameServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Nancy;
    using Data;
    using Data.Model;

    /// <summary>
    /// Create the OData javascript file dynamically.
    /// </summary>
    public class ODataJsModule : NancyModule
    {
        private const string Quote = "\"";
        private string result = "";

        /// <summary>
        /// Creating a module instance.
        /// </summary>
        /// <param name="visualCountryRepository"> The repository for getting the Countries. </param>
        public ODataJsModule(IVisualCountryRepository visualCountryRepository)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader textStreamReader = new StreamReader(assembly.GetManifestResourceStream("GameServer.BaseOData.txt"));
            this.result = textStreamReader.ReadToEnd().Replace("#PlaceHolderDynamicData#", GetJavascriptMultipleCountries(visualCountryRepository.GetAllVisualCountries()));

            Get["/content/oData.js"] = _ =>
            {
                return result;
            };
        }

        /// <summary>
        /// Gets a string in the style
        ///   "Alaska":{ 
        ///     borderPoints:[[342,238],[301,227],[266,207],[223,218],[190,233],[210,269],[170,270],[171,302],[196,300],[168,322]],
        ///     numberCenterPoint:[265,296],
        ///     numberEndPoint:[265,296]
        ///   },
        ///   
        ///   "Alberta":{ 
        ///       borderPoints:[[365,361],[603,369],[588,548],[408,549],[371,465],[383,450],[387,426],[379,396]],
        ///       numberCenterPoint:[491,459],
        ///       numberEndPoint:[491,459]
        ///   },
        /// </summary>
        /// <param name="visualCountries"> The <see cref="VisualCountry"/> instances to create this string from. </param>
        /// <returns> A string with Javascript array code for the countries. </returns>
        private string GetJavascriptMultipleCountries(List<VisualCountry> visualCountries)
        {
            StringBuilder result = new StringBuilder();
            foreach (var visualCountry in visualCountries)
            {
                result.Append(this.GetJavascriptCountry(visualCountry));
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets a string in the style
        ///   "Alaska":{ 
        ///     borderPoints:[[342,238],[301,227],[266,207],[223,218],[190,233],[210,269],[170,270],[171,302],[196,300],[168,322]],
        ///     numberCenterPoint:[265,296],
        ///     numberEndPoint:[265,296]
        ///   },
        /// </summary>
        /// <param name="visualCountry"> The <see cref="VisualCountry"/> instance to create this string from. </param>
        /// <returns> A string with Javascript array code for a Country. </returns>
        private string GetJavascriptCountry(VisualCountry visualCountry)
        {
            StringBuilder result = new StringBuilder();
            result.Append(this.GetJavascriptForCountryName(visualCountry));
            result.Append(this.GetJavascriptForBorderPointsCountry(visualCountry));
            result.Append(this.GetJavascriptForNumberCenterPoint(visualCountry));
            result.Append(this.GetJavascriptForNumberEndPoint(visualCountry));
            return result.ToString();
        }

        /// <summary>
        /// Gets a string in the style
        ///    "Alaska":{ 
        /// </summary>
        /// <param name="visualCountry">The <see cref="VisualCountry"/> instance to create this string from.</param>
        /// <returns> A string with Javascript code for a Country name.</returns>
        private string GetJavascriptForCountryName(VisualCountry visualCountry)
        {
            return "      " + Quote + visualCountry.Name + Quote + ":{" + Environment.NewLine;
        }

        /// <summary>
        /// Gets a string in the style
        ///     borderPoints:[[342,238],[301,227],[266,207],[223,218],[190,233],[210,269],[170,270],[171,302],[196,300],[168,322]],
        /// </summary>
        /// <param name="visualCountry">The <see cref="VisualCountry"/> instance to create this string from.</param>
        /// <returns> A string with Javascript code for borderPoints.</returns>
        private string GetJavascriptForBorderPointsCountry(VisualCountry visualCountry)
        {
            return "        borderPoints:" + this.GetJavascriptPointArray(visualCountry.BorderPoints) + "," + Environment.NewLine;
        }

        /// <summary>
        /// Gets a string in the style
        ///     numberCenterPoint:[265,296],
        /// </summary>
        /// <param name="visualCountry">The <see cref="VisualCountry"/> instance to create this string from.</param>
        /// <returns> A string with Javascript code for NumberCenterPoint.</returns>
        private string GetJavascriptForNumberCenterPoint(VisualCountry visualCountry)
        {
            return "        numberCenterPoint:" + this.GetJavascriptPoint(visualCountry.NumberCenterPoint) + Environment.NewLine;
        }

        /// <summary>
        /// Gets a string in the style
        ///     numberEndPoint:[265,296],
        /// </summary>
        /// <param name="visualCountry">The <see cref="VisualCountry"/> instance to create this string from.</param>
        /// <returns> A string with Javascript code for NumberEndPoint.</returns>
        private string GetJavascriptForNumberEndPoint(VisualCountry visualCountry)
        {
            return "    numberEndPoint:" + this.GetJavascriptPoint(visualCountry.NumberEndPoint) + Environment.NewLine + "  }";
        }

        /// <summary>
        /// Creates a string in the style 
        /// [23,113]
        /// </summary>
        /// <param name="mapPoint"> The <see cref="MapPoint"/> to create the string from. </param>
        /// <returns></returns>
        private string GetJavascriptPoint(MapPoint mapPoint)
        {
            return "[" + mapPoint.X.ToString() + "," + mapPoint.Y.ToString() + "]";
        }

        /// <summary>
        /// Creates a string in the style 
        /// [[342,238],[301,227],[266,207],[223,218],[190,233],[210,269],[170,270],[171,302],[196,300],[168,322]], 
        /// </summary>
        /// <param name="mapPoints"> The list of <see cref="MapPoint"/> instances to create the string from.</param>
        /// <returns> A string with the mappoints as javascript array. </returns>
        private string GetJavascriptPointArray(List<MapPoint> mapPoints)
        {
            StringBuilder result = new StringBuilder();
            result.Append("[");
            foreach (MapPoint mapPoint in mapPoints)
            {
                if (result.Length > 1)
                {
                    result.Append(",");
                }

                result.Append(this.GetJavascriptPoint(mapPoint));
            }

            result.Append("],");
            return result.ToString();
        }
    }
}
