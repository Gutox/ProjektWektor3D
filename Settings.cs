using System.IO;
using System.Text.Json;

namespace ProjektWektor3D
{
    public class SQLSettingsStructure
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }


    public class Settings
    {
        public static string Filename { get; set; }
        public static SQLSettingsStructure SQLSettings { get; set; } = new SQLSettingsStructure();

        public static void Load()
        {
            try
            {
                using (var reader = new StreamReader(Filename))
                {
                    var json = reader.ReadToEnd();
                    SQLSettings = JsonSerializer.Deserialize<SQLSettingsStructure>(json);
                }
            }
            catch (FileNotFoundException)
            {
            }
        }


        public static void Save()
        {
            using (var writer = new StreamWriter(Filename))
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(SQLSettings, options);
                writer.Write(json);
            }
        }
    }
}