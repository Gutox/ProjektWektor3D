using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjektWektor3D.FileHelpers;

namespace ProjektWektor3D
{
    [TestClass]
    public class VectorTests
    {
        public Vector3D Vector1 { get; set; } = new Vector3D(5.25, 2.62, 3.1);
        public Vector3D Vector2 { get; set; } = new Vector3D(0.52, 5.62, 3.11);

        [TestMethod]
        public void Negation()
        {
            Assert.AreEqual(-Vector1, new Vector3D(-5.25, -2.62, -3.1));
        }

        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(Vector1 + Vector2, new Vector3D(5.77, 8.24, 6.21));
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(Vector1 - Vector2, new Vector3D(4.73, -3, -0.01));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(Vector1 * 2, new Vector3D(10.5, 5.24, 6.2));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(Vector1 / 2, new Vector3D(2.625, 1.31, 1.55));
        }

        [TestMethod]
        public void DotProduct()
        {
            Assert.AreEqual(Vector1.DotProduct(Vector2), 27.1, 0.01);
        }

        [TestMethod]
        public void CrossProduct()
        {
            Assert.AreEqual(Vector1.CrossProduct(Vector2), new Vector3D(-9.27, -14.72, 28.14));
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.AreEqual(Vector1.Magnitude(), 6.64, 0.01);
        }

        [TestMethod]
        public void Normalize()
        {
            Assert.AreEqual(Vector1.Normalize(), new Vector3D(0.79, 0.39, 0.46));
        }

        [TestMethod]
        public void Angle()
        {
            Assert.AreEqual(Vector1.Angle(Vector2), 0.88, 0.01);
        }

        [TestMethod]
        public void Distance()
        {
            Assert.AreEqual(Vector1.Distance(Vector2), 5.6, 0.01);
        }
    }

    [TestClass]
    public class FileTests
    {
        public string Filename { get; set; } = "test_settings";

        public Dictionary<string, Vector3D> Vectors { get; set; } = new Dictionary<string, Vector3D>
        {
            { "Vector1", new Vector3D(5.25, 2.62, 3.1) },
            { "Vector2", new Vector3D(0.52, 5.62, 3.11) }
        };

        public bool Helper(IFileHelper fileHelper, string path)
        {
            fileHelper.ExportToFile(path, Vectors);
            var importedVectors = fileHelper.ImportFromFile(path);
            File.Delete(path);
            return !Vectors.Except(importedVectors).Any();
        }

        [TestMethod]
        public void Json()
        {
            var jsonHelper = new JSONHelper();

            var path = $@"{Path.GetTempPath()}\{Filename}.json";
            Assert.IsTrue(Helper(jsonHelper, path));
        }

        [TestMethod]
        public void CSV()
        {
            var csvHelper = new CSVHelper();

            var path = $@"{Path.GetTempPath()}\{Filename}.csv";
            Assert.IsTrue(Helper(csvHelper, path));
        }

        [TestMethod]
        public void XLSX()
        {
            var xlsxHelper = new XLSXHelper();

            var path = $@"{Path.GetTempPath()}\{Filename}.xlsx";
            Assert.IsTrue(Helper(xlsxHelper, path));
        }
    }

    [TestClass]
    public class CommandTests
    {
        public Commands Commands { get; set; } = new Commands();

        public Dictionary<string, Vector3D> Vectors { get; set; } = new Dictionary<string, Vector3D>
        {
            { "v1", new Vector3D(5.25, 2.62, 3.1) },
            { "v2", new Vector3D(0.52, 5.62, 3.11) }
        };


        [TestMethod]
        public void Addition()
        {
            Commands.ParseCommand("v3 = v1 + v2", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(5.77, 8.24, 6.21));
        }

        [TestMethod]
        public void Subtraction()
        {
            Commands.ParseCommand("v3 = v1 - v2", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(4.73, -3, -0.01));
        }

        [TestMethod]
        public void Multiplication()
        {
            Commands.ParseCommand("v3 = v1 * 2", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(10.5, 5.24, 6.2));
        }

        [TestMethod]
        public void Division()
        {
            Commands.ParseCommand("v3 = v1 / 2", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(2.625, 1.31, 1.55));
        }

        [TestMethod]
        public void DotProduct()
        {
            var result = Commands.ParseCommand("v3 = v1 . v2", Vectors);
            if (result == null)
                Assert.Fail("Result is null");

            result = Commands.RemoveWhitespace(result);

            var index = result.IndexOf(@"v3=", StringComparison.Ordinal);
            if (index == -1)
                Assert.Fail();

            var substr = result.Substring(index + 3).Replace(',', '.');
            if (!double.TryParse(substr, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultValue))
                Assert.Fail();

            Assert.AreEqual(resultValue, 27.1, 0.01);
        }

        [TestMethod]
        public void CrossProduct()
        {
            Commands.ParseCommand("v3 = v1 ^ v2", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(-9.27, -14.72, 28.14));
        }

        [TestMethod]
        public void Magnitude()
        {
            var result = Commands.ParseCommand("v3 = |v1|", Vectors);
            if (result == null)
                Assert.Fail("Result is null");

            result = Commands.RemoveWhitespace(result);

            var index = result.IndexOf(@"v3=", StringComparison.Ordinal);
            if (index == -1)
                Assert.Fail();

            var substr = result.Substring(index + 3).Replace(',', '.');
            if (!double.TryParse(substr, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultValue))
                Assert.Fail();

            Assert.AreEqual(resultValue, 6.64, 0.01);
        }

        [TestMethod]
        public void Normalize()
        {
            Commands.ParseCommand("v3 = Normalize(v1)", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(0.79, 0.39, 0.46));
        }

        [TestMethod]
        public void Angle()
        {
            var result = Commands.ParseCommand("v3 = Angle(v1, v2)", Vectors);
            if (result == null)
                Assert.Fail("Result is null");

            result = Commands.RemoveWhitespace(result);

            var index = result.IndexOf(@"v3=", StringComparison.Ordinal);
            if (index == -1)
                Assert.Fail();

            var substr = result.Substring(index + 3).Replace(',', '.');
            if (!double.TryParse(substr, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultValue))
                Assert.Fail();

            Assert.AreEqual(resultValue, 0.88, 0.01);
        }

        [TestMethod]
        public void Distance()
        {
            var result = Commands.ParseCommand("v3 = Distance(v1, v2)", Vectors);
            if (result == null)
                Assert.Fail("Result is null");

            result = Commands.RemoveWhitespace(result);

            var index = result.IndexOf(@"v3=", StringComparison.Ordinal);
            if (index == -1)
                Assert.Fail();

            var substr = result.Substring(index + 3).Replace(',', '.');
            if (!double.TryParse(substr, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultValue))
                Assert.Fail();

            Assert.AreEqual(resultValue, 5.6, 0.01);
        }

        [TestMethod]
        public void Negation()
        {
            Commands.ParseCommand("v3 = Negation(v1)", Vectors);
            Assert.AreEqual(Vectors["v3"], new Vector3D(-5.25, -2.62, -3.1));
        }
    }
}