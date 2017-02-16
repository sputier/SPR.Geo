using static System.Math;

namespace SPR.Geo
{
    public class GpsLambert2Converter
    {
        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_71.pdf 
        /// Calcul de la latitude isométrique sur un ellipsoïde de première excentricité e
        /// au point de latitude ϕ.
        /// </summary>
        /// <param name="phi">Latitude</param>
        /// <param name="e">Première excentricité de l’ellipsoïde</param>
        /// <returns>Latitude isométrique</returns>
        internal double ALG0001(double phi, double e)
        {
            var terme1 = Tan(PI / 4 + phi / 2);
            var eSinPhi = e * Sin(phi);
            var terme2 = (1 - eSinPhi) / (1 + eSinPhi);
            terme2 = Pow(terme2, e / 2);

            return Log(terme1 * terme2);
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_71.pdf 
        /// Calcul de la latitude a partir de la latitude isometrique
        /// </summary>
        /// <param name="li">latitude isometrique</param>
        /// <param name="e">premiere excentricite de l'ellipsoide</param>
        /// <param name="epsilon">Tolérance de convergence</param>
        /// <returns>latitude</returns>
        internal double ALG0002(double li, double e, double epsilon = 1E-11)
        {
            var expLi = Exp(li);

            var phiim1 = 2 * Atan(expLi) - PI / 2;

            var eSinP = e * Sin(phiim1);
            var temp = (1 + eSinP) / (1 - eSinP);
            temp = Pow(temp, e / 2);
            var phii = 2 * Atan(temp * expLi) - PI / 2;

            while (Abs(phii - phiim1) > epsilon)
            {
                eSinP = e * Sin(phii);
                temp = (1 + eSinP) / (1 - eSinP);
                temp = Pow(temp, e / 2);
                var phiip1 = 2 * Atan(temp * expLi) - PI / 2;
                phiim1 = phii;
                phii = phiip1;
            }

            return phii;
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_71.pdf 
        /// Transformation de coordonnees lambda, phi -> X,Y Lambert
        /// </summary>
        /// <param name="e">Premiere excentricite de l'ellipsoide</param>
        /// <param name="n">Exposant de la projection</param>
        /// <param name="c">Constante de la projection</param>
        /// <param name="lambdaC">Longitude de l'origine par rapport au meridien origine</param>
        /// <param name="Xs">Coordonnee X en projection du pole</param>
        /// <param name="Ys">Coordonnee Y en projection du pole</param>
        /// <param name="lambda">Longitude par rapport au meridien origine</param>
        /// <param name="phi">Latitude</param>
        /// <returns>Coordonnées en projection du point</returns>
        internal Lambert2Coordinates ALG0003(double e, double n, double c, double lambdaC, double Xs, double Ys, double lambda, double phi)
        {
            var latitudeIso = ALG0001(phi, e);

            var subExpr01 = c * Exp(-n * latitudeIso);

            var subExpr02 = n * (lambda - lambdaC);

            return new Lambert2Coordinates
            {
                X = Xs + subExpr01 * Sin(subExpr02),
                Y = Ys - subExpr01 * Cos(subExpr02)
            };
        }

        internal class LambdaPhiHe
        {
            /// <summary>
            /// longitude par rapport au meridien origine
            /// </summary>
            public double Lambda { get; set; }

            /// <summary>
            /// latitude
            /// </summary>
            public double Phi { get; set; }

            /// <summary>
            /// hauteur au-dessus de l’ellipsoïde
            /// </summary>
            public double He { get; set; }
        }
        internal class Vector3Double
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_71.pdf 
        /// Transformation de coordonnees X,Y Lambert -> lambda, phi
        /// </summary>
        /// <param name="X">Coordonnee X en projection conique conforme Lambert du point</param>
        /// <param name="Y">Coordonnee Y en projection conique conforme Lambert du point</param>
        /// <param name="n">Exposant de la projection</param>
        /// <param name="c">Constante de la projection</param>
        /// <param name="Xs">coordonnee X en projection du pole</param>
        /// <param name="Ys">coordonnee Y en projection du pole</param>
        /// <param name="lambdaC">Longitude de l'origine par rapport au meridien origine</param>
        /// <param name="e">Premiere excentricite de l'ellipsoide</param>
        /// <returns>
        /// lambda -> longitude par rapport au meridien origine
        /// phi -> latitude
        /// </returns>
        internal LambdaPhiHe ALG0004(double X, double Y, double n, double c, double Xs, double Ys, double lambdaC, double e)
        {
            var r = MathHelpers.Hypot(X - Xs, Y - Ys);
            var gamma = Atan((X - Xs) / (Ys - Y));

            var l = (-1 / n) * Log(Abs(r / c));

            return new LambdaPhiHe
            {
                Lambda = lambdaC + gamma / n,
                Phi = ALG0002(l, e)
            };
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_71.pdf 
        /// Calcul de la grande normale de l’ellipsoïde
        /// </summary>
        /// <param name="phi">Latitude</param>
        /// <param name="a">Demi-grand axe de l’ellipsoïde</param>
        /// <param name="e">Première excentricité de l’ellipsoïde.</param>
        /// <returns>Grande normale</returns>
        internal double ALG0021(double phi, double a, double e)
        {
            return a / Sqrt(1 - Pow(e * Sin(phi), 2));
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_80.pdf 
        /// Transformation de coordonnees geographiques ellipsoidale en coordonnees cartesiennes
        /// </summary>
        /// <param name="lambda">Longitude par rapport au meridien origine</param>
        /// <param name="phi">Latitude</param>
        /// <param name="he">Hauteur au dessus de l'ellipsoide</param>
        /// <param name="a">Demi grand axe de l'ellipsoide</param>
        /// <param name="e">Premiere excentricite de l'ellipsoide</param>
        /// <returns>Coordonnees cartesiennes</returns>
        internal Vector3Double ALG0009(double lambda, double phi, double he, double a, double e)
        {
            var n = ALG0021(phi, a, e);
            var nHeCosPhi = (n + he) * Cos(phi);

            return new Vector3Double
            {
                X = (n + he) * Cos(phi) * Cos(lambda),
                Y = (n + he) * Cos(phi) * Sin(lambda),
                Z = (n * (1 - e * e) + he) * Sin(phi)
            };
        }

        internal double ALG0012_Subex01(double e, double phi)
        {
            return Sqrt(1 - (e * Sin(phi)) * (e * Sin(phi)));
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_80.pdf
        /// Transformation, pour une ellipsoide donne, des coordonnees cartesiennes cartesiennes d'un point en coordonnees geographiques, par la methode de Heiskanen-Moritz-Boucher
        /// </summary>
        /// <param name="a">Demi grand axe de l'ellipsoide</param>
        /// <param name="e">Premiere excentricite de l'ellipsoide</param>
        /// <param name="X">Coordonnee cartesienne X</param>
        /// <param name="Y">Coordonnee cartesienne Y</param>
        /// <param name="Z">Coordonnee cartesienne Z</param>
        /// <returns>
        /// lambda -> longitude par rapport au meridien origine
        /// phi -> latitude
        /// he -> hauteur au dessus de l'ellipsoide
        /// </returns>
        internal LambdaPhiHe ALG0012(double a, double e, double X, double Y, double Z, double epsilon = 1E-11)
        {
            var r2 = MathHelpers.Hypot(X, Y);
            var r3 = MathHelpers.Hypot(r2, Z);
            var ae2 = a * e * e;

            var phi0 = Atan(Z / (r2 * (1 - ae2 / r3)));
            var phi1 = Atan((Z / r2) / (1 - ae2 * Cos(phi0) / (r2 * ALG0012_Subex01(e, phi0))));

            while (Abs(phi1 - phi0) > epsilon)
            {
                phi1 = Atan((Z / r2) * 1 / (1 - ae2 * Cos(phi0) / (r2 * ALG0012_Subex01(e, phi0))));
                phi0 = phi1;
            }

            return new LambdaPhiHe
            {
                Lambda = Atan(Y / X),
                Phi = phi1,
                He = (r2 / Cos(phi1)) - a / ALG0012_Subex01(e, phi1)
            };
        }

        /// <summary>        
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/notice/NTG_80.pdf
        /// A partir d’un jeu de 7 paramètres (3 translations, 1 facteur d’échelle et
        /// 3 rotations) de passage du système(1) vers le système(2), et des coordonnées
        /// cartésiennes tridimensionnelles dans le système(1), calcul des coordonnées
        /// cartésiennes tridimensionnelles dans le système(2)
        /// </summary>
        /// <param name="t">Translation (de 1 vers 2)</param>
        /// <param name="d">facteur d'echelle (de 1 vers 2)</param>
        /// <param name="r">angle de rotation (de 1 vers 2)</param>
        /// <param name="u">vecteur de coordonnées cartésiennes tridimension-nellesdans le système(1)</param>
        /// <returns>vecteur de coordonnées cartésiennes tridimension-nelles dans le système(2)</returns>
        internal Vector3Double ALG0013(Vector3Double t, double d, Vector3Double r, Vector3Double u)
        {
            var dp1 = 1 + d;

            return new Vector3Double
            {
                X = t.X + u.X * dp1 + u.Z * r.Y - u.Y * r.Z,
                Y = t.Y + u.Y * dp1 + u.X * r.Z - u.Z * r.X,
                Z = t.Z + u.Z * dp1 + u.Y * r.X - u.X * r.Y
            };
        }

        /// <summary>
        /// Algorithme IGN : http://geodesie.ign.fr/contenu/fichiers/documentation/algorithmes/alg0063.pdf
        /// A partir d’un jeu de 7 paramètres (3 translations, 1 facteur d’échelle et
        /// 3 rotations) de passage du système(1) vers le système(2), et des coordonnées
        /// cartésiennes tridimensionnelles dans le système(2), calcul des coordonnées
        /// cartésiennes tridimensionnelles dans le système(1)
        /// </summary>
        /// <param name="t">Translation (de 1 vers 2)</param>
        /// <param name="d">facteur d'echelle (de 1 vers 2)</param>
        /// <param name="r">angle de rotation (de 1 vers 2)</param>
        /// <param name="v">vecteur de coordonnées cartésiennes tridimension-nelles dans le système(2)</param>
        /// <returns>vecteur de coordonnées cartésiennes tridimension-nelles dans le système(1)</returns>
        internal Vector3Double ALG0063(Vector3Double t, double d, Vector3Double r, Vector3Double v)
        {
            var w = new Vector3Double();
            w.X = v.X - t.X;
            w.Y = v.Y - t.Y;
            w.Z = v.Z - t.Z;
            var e = 1 + d;
            var det = e * (e * e + r.X * r.X + r.Y * r.Y + r.Z * r.Z);

            var e2Rx2 = e * e + r.X * r.X;
            var e2Ry2 = e * e + r.Y * r.Y;
            var e2Rz2 = e * e + r.Z * r.Z;
            var RxRy = r.X * r.Y;
            var RyRz = r.Y * r.Z;
            var RxRz = r.Z * r.X;

            var result = new Vector3Double();

            result.X = e2Rx2 * w.X + (RxRy + e * r.Z) * w.Y + (RxRz - e * r.Y) * w.Z;
            result.Y = (RxRy - e * r.Z) * w.X + e2Ry2 * w.Y + (RyRz + e * r.X) * w.Z;
            result.Z = (RxRz + e * r.Y) * w.X + (RyRz - e * r.X) * w.Y + e2Rz2 * w.Z;

            result.X /= det;
            result.Y /= det;
            result.Z /= det;

            return result;
        }

        public Lambert2Coordinates GpsToLambert2(double latitude, double longitude)
        {
            /*                                    PARAMETRES DE CONVERSION LAMBERT
            ' |---------------------------------------------------------------------------------------------------------------|
            ' | Const | 1 'Lambert I | 2 'Lambert II | 3 'Lambert III | 4 'Lambert IV | 5 'Lambert II Etendue | 6 'Lambert 93 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    n  | 0.7604059656 |  0.7289686274 |   0.6959127966 | 0.6712679322  |    0.7289686274       |  0.7256077650 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    c  | 11603796.98  |  11745793.39  |   11947992.52  | 12136281.99   |    11745793.39        |  11754255.426 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    Xs |   600000.0   |    600000.0   |   600000.0     |      234.358  |    600000.0           |     700000.0  |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    Ys | 5657616.674  |  6199695.768  |   6791905.085  |  7239161.542  |    8199695.768        | 12655612.050  |
            ' |---------------------------------------------------------------------------------------------------------------|
            */

            longitude = longitude * PI / 180;
            latitude = latitude * PI / 180;

            //Conversion WGS84 géographique -> WGS84 cartésien : ALG0009
            // Ellipsoide WGS84
            var a = 6378137.0;
            var f = 1 / 298.257223563;
            var b = a * (1 - f);
            var e = Sqrt((a * a - b * b) / (a * a));

            var he = 0.0;

            var wgs84Cartesian = ALG0009(longitude, latitude, he, a, e);

            //Conversion WGS84 cartésien -> NTF cartésien : ALG063
            var d = 0.00;
            var r = new Vector3Double
            {
                X = 0.00,
                Y = 0.00,
                Z = 0.00
            };
            var t = new Vector3Double
            {
                X = -168.00,
                Y = -060.00,
                Z = 320
            };

            var ntfCartesian = ALG0063(t, d, r, wgs84Cartesian);

            // Ellipsoide Clarke 1880
            a = 6378249.2;
            f = 1 / 293.466021;
            b = a * (1 - f);
            e = Sqrt((a * a - b * b) / (a * a));

            //Conversion NTF cartésien -> NTF géographique : ALG012
            var ntfGeo = ALG0012(a, e, ntfCartesian.X, ntfCartesian.Y, ntfCartesian.Z);

            //Conversion NTF géographique -> Lambert : ALG003

            //Paramètres Lambert 2 étendue
            var n = 0.7289686274;
            var c = 11745793.39;

            var Xs = 600000;
            var Ys = 8199695.768;

            var lambdaC = 2.337229167 * PI / 180.00;

            var lambert = ALG0003(e, n, c, lambdaC, Xs, Ys, ntfGeo.Lambda, ntfGeo.Phi);

            return lambert;
        }

        public GpsCoordinates Lambert2ToGps(double XLambert, double YLambert)
        {
            /*                                    PARAMETRES DE CONVERSION LAMBERT
            ' |---------------------------------------------------------------------------------------------------------------|
            ' | Const | 1 'Lambert I | 2 'Lambert II | 3 'Lambert III | 4 'Lambert IV | 5 'Lambert II Etendue | 6 'Lambert 93 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    n  | 0.7604059656 |  0.7289686274 |   0.6959127966 | 0.6712679322  |    0.7289686274       |  0.7256077650 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    c  | 11603796.98  |  11745793.39  |   11947992.52  | 12136281.99   |    11745793.39        |  11754255.426 |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    Xs |   600000.0   |    600000.0   |   600000.0     |      234.358  |    600000.0           |     700000.0  |
            ' |-------|--------------|---------------|----------------|---------------|-----------------------|---------------|
            ' |    Ys | 5657616.674  |  6199695.768  |   6791905.085  |  7239161.542  |    8199695.768        | 12655612.050  |
            ' |---------------------------------------------------------------------------------------------------------------|
            */
            
            //Conversion Lambert -> NTF geographique : ALG004
            // Ellipsoide Clarke 1880
            var a = 6378249.2;
            var f = 1 / 293.466021;
            var b = a * (1 - f);
            var e = Sqrt((a * a - b * b) / (a * a));

            var he = 0.0;

            //Paramètre Lambert 2 Etendu
            var n = 0.7289686274;
            var c = 11745793.39;

            var Xs = 600000;
            var Ys = 8199695.768;

            var lambdaC = 2.337229167 * PI / 180.00;
            var r1 = ALG0004(XLambert, YLambert, n, c, Xs, Ys, lambdaC, e);

            //Conversion NTF géographique -> NTF cartésien : ALG0009        
            var ntfCartesian = ALG0009(r1.Lambda, r1.Phi, he, a, e);

            //Conversion NTF cartésien -> WGS84 cartésien : ALG013
            var d = 0.00;
            var r = new Vector3Double
            {
                X = 0.00,
                Y = 0.00,
                Z = 0.00
            };
            var t = new Vector3Double
            {
                X = -168.00,
                Y = -060.00,
                Z = 320
            };

            var wgs84Cartesian = ALG0013(t, d, r, ntfCartesian);

            // Ellipsoide WGS84
            a = 6378137.0;
            f = 1 / 298.257223563;
            b = a * (1 - f);
            e = Sqrt((a * a - b * b) / (a * a));

            //Conversion WGS84 cartésien -> WGS84 géographique : ALG012
            var lambdaPhi = ALG0012(a, e, wgs84Cartesian.X, wgs84Cartesian.Y, wgs84Cartesian.Z);

            //Conversion radian -> degre-minute-seconde-orientation
            return new GpsCoordinates
            {
                Longitude = lambdaPhi.Lambda * 180 / PI,
                Latitude = lambdaPhi.Phi * 180 / PI
            };
        }
    }
}
