using System.Collections.Generic;

namespace ProjektWektor3D.FileHelpers
{
    public interface IFileHelper
    {
        Dictionary<string, Vector3D> ImportFromFile(string path);
        void ExportToFile(string path, Dictionary<string, Vector3D> vectors);
    }
}