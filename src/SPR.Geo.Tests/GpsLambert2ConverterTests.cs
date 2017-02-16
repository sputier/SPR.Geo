using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.Geo;

namespace SPR.Geo.Tests
{
    [TestClass]
    public class GpsLambert2ConverterTests
    {
        [TestMethod]
        public void TestALG0001()
        {
            /* ___________________________________________________________________
              |                        JEU DE TEST IGN                            |
              |___________________________________________________________________|
              | ϕ(rad) | 0,872 664 626 00 | -0,300 000 000 00 | 0,199 989 033 70  |
              |________|__________________|___________________|___________________|
              | e      | 0,081 991 889 98 | 0,081 991 889 98  | 0,081 991 889 98  |
              |________|__________________|___________________|___________________|
              |________|__________________|___________________|___________________|
              | L      | 1,005 526 536 49 | -0,302 616 900 63 | 0,200 000 000 009 |
              |________|__________________|___________________|___________________|            */

            var converter = new GpsLambert2Converter();

            var inputPhi = 0.87266462600;
            var inputE = 0.08199188998;
            var expected = 1.00552653649;

            var result = converter.ALG0001(inputPhi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 11));

            inputPhi = -0.30000000000;
            inputE = 0.08199188998;
            expected = -0.30261690063;

            result = converter.ALG0001(inputPhi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 11));

            inputPhi = 0.19998903370;
            inputE = 0.08199188998;
            expected = 0.200000000009;

            result = converter.ALG0001(inputPhi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 12));
        }

        [TestMethod]
        public void TestALG0002()
        {
            /* ___________________________________________________________________
              |                        JEU DE TEST IGN                            |
              |___________________________________________________________________|
              | L      | 1,005 526 536 48 | -0,302 616 900 60 | 0,200 000 000 0   |
              |________|__________________|___________________|___________________|
              | e      | 0,081 991 889 98 | 0,081 991 889 98  | 0,081 991 889 98  |
              |________|__________________|___________________|___________________|
              | ε      | 1.10-11          | 1.10-11           | 1.10-11           |
              |________|__________________|___________________|___________________|
              |________|__________________|___________________|___________________|
              | ϕ(rad) | 0,872 664 626 00 | -0,299 999 999 97 | 0,199 989 033 69  |
              |________|__________________|___________________|___________________|            */

            var converter = new GpsLambert2Converter();

            var inputLi = 1.00552653648;
            var inputE = 0.08199188998;
            var expected = 0.87266462600;

            var result = converter.ALG0002(inputLi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 11));

            inputLi = -0.30261690060;
            inputE = 0.08199188998;
            expected = -0.29999999997;

            result = converter.ALG0002(inputLi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 11));

            inputLi = 0.2000000000;
            inputE = 0.08199188998;
            expected = 0.19998903369;

            result = converter.ALG0002(inputLi, inputE);
            Assert.AreEqual(expected, Math.Round(result, 11));
        }

        [TestMethod]
        public void TestALG0003()
        {
            /* _____________________________
              |       JEU DE TEST IGN       |
              |_____________________________|
              | e        | 0,082 483 256 8  |
              |__________|__________________|
              | n        | 0,760 405 966    |
              |__________|__________________|
              | c (m)    | 11 603 796,976 7 |
              |__________|__________________|
              | λc (rad) | 0,040 792 344 33 |
              |__________|__________________|
              | Xs (m)   | 600 000,000 0    |
              |__________|__________________|
              | Ys (m)   | 5 657 616,674 0  |
              |__________|__________________|
              | λ (rad)  | 0,145 512 099 00 |
              |__________|__________________|
              | ϕ (rad)  | 0,872 664 626 00 |
              |__________|__________________|
              |__________|__________________|
              | X (m)    | 1 029 705,081 8  |
              |__________|__________________|
              | Y (m)    | 272 723,851 0    |
              |__________|__________________|            */

            var converter = new GpsLambert2Converter();

            var inputE = 0.0824832568;
            var inputN = 0.760405966;
            var inputC = 11603796.9767;
            var inputLambdaC = 0.04079234433;
            var inputXs = 600000.0000;
            var inputYs = 5657616.6740;
            var inputLambda = 0.14551209900;
            var inputPhi = 0.87266462600;
            var expectedX = 1029705.0818;
            var expectedY = 272723.8510;

            var result = converter.ALG0003(inputE, inputN, inputC, inputLambdaC, inputXs,
                                           inputYs, inputLambda, inputPhi);

            Assert.AreEqual(expectedX, Math.Round(result.X, 4));            Assert.AreEqual(expectedY, Math.Round(result.Y, 4));
        }

        [TestMethod]
        public void TestALG0004()
        {
            /* _____________________________
              |       JEU DE TEST IGN       |
              |_____________________________|
              | X (m)    | 1 029 705,083 0  |
              |__________|__________________|
              | Y (m)    | 272 723,849 0    |
              |__________|__________________|
              | n        | 0,760 405 966    |
              |__________|__________________|
              | c (m)    | 11 603 796,976 7 |
              |__________|__________________|
              | Xs (m)   | 600 000,000 0    |
              |__________|__________________|
              | Ys (m)   | 5 657 616,674 0  |
              |__________|__________________|
              | λc (rad) | 0,040 792 344 33 |
              |__________|__________________|
              | e        | 0,082 483 256 8  |
              |__________|__________________|
              | ε        | 1.10-11          |
              |__________|__________________|
              |__________|__________________|
              | λ (rad)  | 0,145 512 099 25 |
              |__________|__________________|
              | ϕ (rad)  | 0,872 664 625 67 |
              |__________|__________________|            */

            var converter = new GpsLambert2Converter();

            var inputX = 1029705.0830;
            var inputY = 272723.8490;
            var inputN = 0.760405966;
            var inputC = 11603796.9767;
            var inputXs = 600000.0000;
            var inputYs = 5657616.6740;
            var inputLambdaC = 0.04079234433;
            var inputE = 0.0824832568;

            var expectedLambda = 0.14551209925;
            var expectedPhi = 0.87266462567;

            var result = converter.ALG0004(inputX, inputY, inputN, inputC, inputXs,
                                           inputYs, inputLambdaC, inputE);

            Assert.AreEqual(expectedLambda, Math.Round(result.Lambda, 11));            Assert.AreEqual(expectedPhi, Math.Round(result.Phi, 11));
        }

        [TestMethod]
        public void TestALG0021()
        {
            /* _____________________________
              |       JEU DE TEST IGN       |
              |_____________________________|
              | ϕ (rad)  | 0,977 384 381 00 |
              |__________|__________________|
              | a(m)     | 6 378 388,000 0  |
              |__________|__________________|
              | e        | 0,081 991 890    |
              |__________|__________________|
              |__________|__________________|
              | N(m)     | 6 393 174,975 5  |
              |__________|__________________|
            */

            var converter = new GpsLambert2Converter();

            var inputPhi = 0.97738438100;
            var inputA = 6378388.0000;
            var inputE = 0.081991890;
            var expected = 6393174.9755;

            var result = converter.ALG0021(inputPhi, inputA, inputE);
            Assert.AreEqual(expected, Math.Round(result, 4));
        }

        [TestMethod]
        public void TestALG0009()
        {
            /* ___________________________________________________________________
              |                        JEU DE TEST IGN                            |
              |___________________________________________________________________|
              | λ(rad) | 0,017 453 292 48 | 0,002 908 882 12  | 0,005 817 764 23  |
              |________|__________________|___________________|___________________|
              | ϕ(rad) | 0,020 362 174 57 | 0,000 000 000 00  | -0,031 997 703 00 |
              |________|__________________|___________________|___________________|
              | he (m) | 100,000 0        | 10,000 0          | 2 000,000 0       |
              |________|__________________|___________________|___________________|
              | a (m)  | 6 378 249,200 0  | 6 378 249,200 0   | 6 378 249,200 0   |
              |________|__________________|___________________|___________________|
              | e      | 0,082 483 256 79 | 0,082 483 256 79  | 0,082 483 256 79  |
              |________|__________________|___________________|___________________|
              |________|__________________|___________________|___________________|
              | X(m)   | 6 376 064,695 5  | 6 378 232,214 9   | 6 376 897,536 9   |
              |________|__________________|___________________|___________________|
              | Y(m)   | 111 294,623 0    | 18 553,578 0      | 37 099,705 0      |
              |________|__________________|___________________|___________________|
              | Z(m)   | 128 984,725 0    | 0,000 0           | -202 730,907 0    |
              |________|__________________|___________________|___________________|            */

            var converter = new GpsLambert2Converter();

            var inputLambda = 0.01745329248;
            var inputPhi = 0.02036217457;
            var inputHe = 100.0000;
            var inputA = 6378249.2000;
            var inputE = 0.08248325679;
            var expectedX = 6376064.6955;
            var expectedY = 111294.6230;
            var expectedZ = 128984.7250;

            var result = converter.ALG0009(inputLambda, inputPhi, inputHe, inputA, inputE);
            Assert.AreEqual(expectedX, Math.Round(result.X, 4));
            Assert.AreEqual(expectedY, Math.Round(result.Y, 4));
            Assert.AreEqual(expectedZ, Math.Round(result.Z, 4));

            inputLambda = 0.00290888212;
            inputPhi = 0.00000000000;
            inputHe = 10.0000;
            inputA = 6378249.2000;
            inputE = 0.08248325679;
            expectedX = 6378232.2149;
            expectedY = 18553.5780;
            expectedZ = 0.0000;

            result = converter.ALG0009(inputLambda, inputPhi, inputHe, inputA, inputE);
            Assert.AreEqual(expectedX, Math.Round(result.X, 4));
            Assert.AreEqual(expectedY, Math.Round(result.Y, 4));
            Assert.AreEqual(expectedZ, Math.Round(result.Z, 4));

            inputLambda = 0.00581776423;
            inputPhi = -0.03199770300;
            inputHe = 2000.0000;
            inputA = 6378249.2000;
            inputE = 0.08248325679;
            expectedX = 6376897.5369;
            expectedY = 37099.7050;
            expectedZ = -202730.9070;

            result = converter.ALG0009(inputLambda, inputPhi, inputHe, inputA, inputE);
            Assert.AreEqual(expectedX, Math.Round(result.X, 4));
            Assert.AreEqual(expectedY, Math.Round(result.Y, 4));
            Assert.AreEqual(expectedZ, result.Z, 1E-4);
        }

        [TestMethod]
        public void TestALG0012()
        {
            /* ___________________________________________________________________
              |                        JEU DE TEST IGN                            |
              |___________________________________________________________________|
              | a (m)  | 6 378 249,200 0  | 6 378 249,200 0   | 6 378 249,200 0   |
              |________|__________________|___________________|___________________|
              | e      | 0,082 483 256 79 | 0,082 483 256 79  | 0,082 483 256 79  |
              |________|__________________|___________________|___________________|
              | X (m)  | 6 376 064,695 0  | 6 378 232,215 0   | 6 376 897,537 0   |
              |________|__________________|___________________|___________________|
              | Y (m)  | 111 294,623 0    | 18 553,578 0      | 37 099,705 0      |
              |________|__________________|___________________|___________________|
              | Z (m)  | 128 984,725 0    | 0,000 0           | -202 730,907 0    |
              |________|__________________|___________________|___________________|
              | ε(rad) | 1 x 10-11        | 1 x 10-11         | 1 x 10-11         |
              |________|__________________|___________________|___________________|
              |________|__________________|___________________|___________________|
              | λ(rad) | 0,017 453 292 48 | 0,002 908 882 12  | 0,005 817 764 23  |
              |________|__________________|___________________|___________________|
              | ϕ(rad) | 0,020 362 174 57 | 0,000 000 000 00  | -0,031 997 703 01 |
              |________|__________________|___________________|___________________|
              | he (m) | 99,999 5         | 10,000 1          | 2 000,000 1       |
              |________|__________________|___________________|___________________|            */

            var converter = new GpsLambert2Converter();

            var inputA = 6378249.2000;
            var inputE = 0.08248325679;
            var inputX = 6376064.6950;
            var inputY = 111294.6230;
            var inputZ = 128984.7250;
            var expectedLambda = 0.01745329248;
            var expectedPhi = 0.02036217457;
            var expectedHe = 99.9995;

            var result = converter.ALG0012(inputA, inputE, inputX, inputY, inputZ);
            Assert.AreEqual(expectedLambda, Math.Round(result.Lambda, 11));
            Assert.AreEqual(expectedPhi, Math.Round(result.Phi, 11));
            Assert.AreEqual(expectedHe, Math.Round(result.He, 4));

            inputA = 6378249.2000;
            inputE = 0.08248325679;
            inputX = 6378232.2150;
            inputY = 18553.5780;
            inputZ = 0.0000;
            expectedLambda = 0.00290888212;
            expectedPhi = 0.00000000000;
            expectedHe = 10.0001;

            result = converter.ALG0012(inputA, inputE, inputX, inputY, inputZ);
            Assert.AreEqual(expectedLambda, Math.Round(result.Lambda, 11));
            Assert.AreEqual(expectedPhi, Math.Round(result.Phi, 11));
            Assert.AreEqual(expectedHe, Math.Round(result.He, 4));

            inputA = 6378249.2000;
            inputE = 0.08248325679;
            inputX = 6376897.5370;
            inputY = 37099.7050;
            inputZ = -202730.9070;
            expectedLambda = 0.00581776423;
            expectedPhi = -0.03199770301;
            expectedHe = 2000.0001;

            result = converter.ALG0012(inputA, inputE, inputX, inputY, inputZ);
            Assert.AreEqual(expectedLambda, result.Lambda, 1E-11);
            Assert.AreEqual(expectedPhi, Math.Round(result.Phi, 11));
            Assert.AreEqual(expectedHe, Math.Round(result.He, 4));
        }


    }
}
