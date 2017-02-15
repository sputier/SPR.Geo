using static System.Math;

namespace SPR.Geo
{
    public class GpsCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        /// <summary>
        /// Calcule la disance en km entre deux coordonnées GPS
        /// </summary>
        /// <param name="coordinates2">Coordonnées du second point</param>
        /// <returns>Distance en km</returns>
        public double DistanceTo(GpsCoordinates coordinates2)
        {
            var coordinates1 = this;

            var earthRadius = 6371;

            var deltaLat = MathHelpers.ConvertDegreesToRadians(coordinates2.Latitude - coordinates1.Latitude);
            var deltaLong = MathHelpers.ConvertDegreesToRadians(coordinates2.Longitude - coordinates1.Longitude);

            var temp = Pow(Sin(deltaLat / 2), 2)
                  + (Cos(MathHelpers.ConvertDegreesToRadians(coordinates1.Latitude))
                        * Cos(MathHelpers.ConvertDegreesToRadians(coordinates2.Latitude))
                        * Pow(Sin(deltaLong / 2), 2));

            return 2 * earthRadius * Asin(temp);
        }
    }
}
