using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ProjektWektor3D.FileHelpers
{
    internal class CSVHelper : IFileHelper
    {
        public Dictionary<string, Vector3D> ImportFromFile(string path)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("csv"))
                throw new Exception("Invalid file path or extension!");

            var vectors = new Dictionary<string, Vector3D>();

            using (var parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(":");
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (fields == null || fields.Length != 4)
                        throw new Exception("File is contains invalid data!");

                    var name = fields[0];

                    var x = double.Parse(fields[1]);
                    var y = double.Parse(fields[2]);
                    var z = double.Parse(fields[3]);

                    vectors[name] = new Vector3D(x, y, z);
                }
            }

            return vectors;
        }

        public void ExportToFile(string path, Dictionary<string, Vector3D> vectors)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("csv"))
                throw new Exception("Invalid file path or extension!");

            using (var writer = new StreamWriter(path))
            {
                foreach (var vector in vectors)
                    writer.WriteLine($"{vector.Key}:{vector.Value.X}:{vector.Value.Y}:{vector.Value.Z}");
            }
        }
    }
}