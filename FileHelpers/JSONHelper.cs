using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProjektWektor3D.FileHelpers
{
    internal class JSONHelper : IFileHelper
    {
        public Dictionary<string, Vector3D> ImportFromFile(string path)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("json"))
                throw new Exception("Invalid file path or extension!");

            using (var reader = new StreamReader(path))
            {
                var json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<Dictionary<string, Vector3D>>(json);
            }
        }

        public void ExportToFile(string path, Dictionary<string, Vector3D> vectors)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("json"))
                throw new Exception("Invalid file path or extension!");

            using (var writer = new StreamWriter(path))
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(vectors, options);
                writer.Write(json);
            }
        }
    }
}