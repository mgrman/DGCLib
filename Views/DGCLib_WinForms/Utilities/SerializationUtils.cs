using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DGCLib_WinForms.Utilities
{
    public static class SerializationUtils
    {
        public static string Serialize(object obj)
        {
            string xml = "";
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());

                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t"
                };

                var writer = XmlWriter.Create(memoryStream, settings);

                serializer.WriteObject(writer, obj);
                writer.Flush();
                xml = Encoding.UTF8.GetString(memoryStream.ToArray());

                //serializer.WriteObject(memoryStream, obj);
            }
            return xml;
        }

        public static T Deserialize<T>(string xml)
        {
            return (T)Deserialize(xml, typeof(T));
        }

        public static object Deserialize(string xml, Type type)
        {
            if (string.IsNullOrEmpty(xml))
                return null;
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                quotas.MaxArrayLength = int.MaxValue;
                quotas.MaxDepth = int.MaxValue;

                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, quotas, null);

                DataContractSerializer serializer = new DataContractSerializer(type);
                var obj = serializer.ReadObject(reader);
                return obj;
            }
        }

        public static object Clone(object obj)
        {
            return Deserialize(Serialize(obj), obj.GetType());
        }
    }
}