using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    class Tools
    {
        /// <summary>
        /// Serialize data to file
        /// </summary>
        /// <param name="filePath">Path with filename</param>
        /// <param name="data">Data to serialize</param>
        public static void SerializeToFile(String filePath, Object data)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        /// <summary>
        /// Deserialize data from file
        /// </summary>
        /// <param name="filePath">Path with filename</param>
        /// <returns>Data from file</returns>
        public static object DeserializeFromFile(String filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            BinaryFormatter formatter = new BinaryFormatter();
            Object data = formatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
    }
}
