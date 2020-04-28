using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace VotacaoApi.Models.Arquivos
{
    public class Arquivos
    {
        public static T Deserialize<T>(string path)
        {
            if (!System.IO.File.Exists(path)) return default(T);

            StreamReader file = new StreamReader(path);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            T result = (T)xs.Deserialize(file);
            file.Close();
            file.Dispose();

            return result;
        }
        public static void Serialize<T>(T r, string path)
        {
            StreamWriter file = new StreamWriter(path);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(file, r);
            file.Close();
            file.Dispose();
        }
    }
}