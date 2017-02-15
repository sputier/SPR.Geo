using static System.Math;

namespace SPR.Geo
{
    public static class MathHelpers
    {
        public static double ConvertDegreesToRadians(double degrees)
        {
            return degrees * PI / 180.0;
        }
        public static double ConvertRadiansToDegrees(double radians)
        {
            return radians * 180.0 / PI;
        }

        /// <summary>
        ///  Calcule la longueur de l'hypoténuse d'un triangle à angle droit
        /// </summary>
        /// <param name="val1">Longueur du côté 1</param>
        /// <param name="val2">Longueur du côté 2</param>
        /// <returns>Longueur de l'hypothénuse</returns>
        public static double Hypot(double val1, double val2)
        {
            return Sqrt(val1 * val1 + val2 * val2);
        }
    }
}
