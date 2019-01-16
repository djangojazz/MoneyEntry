using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MoneyEntry.ExpensesAPI
{
    static class SerializationHelper
    {
        public static bool ValidateXml(this string xmlInput)
        {
            try
            {
                XElement.Parse(xmlInput);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static string SerializeToXml<T>(this T valueToSerialize, string namespaceUsed = null)
        {
            var ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, (namespaceUsed != null) ? namespaceUsed : string.Empty) });
            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    dynamic xmler = new XmlSerializer(typeof(T));
                    xmler.Serialize(writer, valueToSerialize, ns);
                }

                return sw.ToString();
            }
        }
    }
}
