using Controls.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Controls
{

  #region ClearAndAddMethods
  public static class ExtensionMethods
  {                 
    public static void ClearAndAddRange<T>(this IList<T> input, IEnumerable<T> array)
    {
      input.Clear();
      foreach (var o in array)
      {                        
        input.Add(o);
      }
    }

    public static void ClearAndAddRange<T>(this ObservableCollection<T> input, IEnumerable<T> array)
    {
      input.Clear();
      foreach (var o in array)
      {
        input.Add(o);
      }
    }

    public static void ClearAndAddRange<T>(this ObservableCollectionContentNotifying<T> input, IEnumerable<T> array)
    {
      input.SuspendNotification = true;
      input.Clear();
      foreach (var o in array)
      {
        input.Add(o);
      }
      input.SuspendNotification = false;
    }

    public static void ClearAndAddRange<T, T2>(this IDictionary<T, T2> input, IDictionary<T, T2> dict)
    {
      input.Clear();
      foreach (var o in dict)
      {                      
        input.Add(o);
      }
    }
    #endregion

    #region XMLSerialization
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

    private static HashSet<Type> ConstructedSerializers = new HashSet<Type>();

    public static string SerializeToXml<T>(this T valueToSerialize)
    {
      var ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) });
      ns.Add("", "");
      using (var sw = new StringWriter())
      {
        using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
        {
          dynamic xmler = GetXmlSerializer(typeof(T));
          xmler.Serialize(writer, valueToSerialize, ns);
        }

        return sw.ToString();
      }
    }

    public static T DeserializeXml<T>(this string xmlToDeserialize)
    {
      dynamic serializer = new XmlSerializer(typeof(T));

      using (TextReader reader = new StringReader(xmlToDeserialize))
      {
        return (T)serializer.Deserialize(reader);
      }
    }

    public static XmlSerializer GetXmlSerializer(Type typeToSerialize)
    {
      if (!ConstructedSerializers.Contains(typeToSerialize))
      {
        ConstructedSerializers.Add(typeToSerialize);
        return XmlSerializer.FromTypes(new Type[] { typeToSerialize })[0];
      }
      else
      {
        return new XmlSerializer(typeToSerialize);
      }
    }
    #endregion

    public static bool ContainsInvariant(this string input, string contains)
    {
      return input.ToUpper().Trim().Contains(contains.ToUpper().Trim());
    }
                            
    public static string DisplayNumberWithStringSuffix(this int inputNumber)
    {
      if (inputNumber.ToString().EndsWith("11") | inputNumber.ToString().EndsWith("12") | inputNumber.ToString().EndsWith("13"))
        return inputNumber.ToString() + "th";
      if ((inputNumber.ToString().EndsWith("1")))
        return inputNumber.ToString() + "st";
      if ((inputNumber.ToString().EndsWith("2")))
        return inputNumber.ToString() + "nd";
      if ((inputNumber.ToString().EndsWith("3")))
        return inputNumber.ToString() + "rd";
      return inputNumber.ToString() + "th";
    }
                           
    public static T DeepClone<T>(this T obj)
    {
      dynamic Formater = new BinaryFormatter();
      System.IO.MemoryStream m = new System.IO.MemoryStream();
      Formater.Serialize(m, obj);
      m.Seek(0, System.IO.SeekOrigin.Begin);
      return (T)Formater.Deserialize(m);
    }

    public static string GetStringListings(this IDictionary dictionary)
    {
      string s = string.Empty;
      foreach (DictionaryEntry o in dictionary) { s += $"{o.Key.ToString()} {o.Value.ToString()} {Environment.NewLine}"; }
      return s.Trim();
    }
  }
}
