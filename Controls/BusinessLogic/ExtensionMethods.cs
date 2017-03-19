using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
namespace Controls.BusinessLogic
{
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
                      
    //public static void ClearAndAddRange<T>(this ObservableCollectionContentNotifying<T> input, IEnumerable<T> array)
    //{
    //  input.SuspendNotification = true;
    //  input.Clear();    
    //  foreach (var o in array)
    //  {                         
    //    input.Add(o);
    //  }
    //  input.SuspendNotification = false;
    //}
                     
    public static void ClearAndAddRange<T, T2>(this IDictionary<T, T2> input, IDictionary<T, T2> dict)
    {
      input.Clear();
      foreach (var o in dict)
      {                      
        input.Add(o);
      }
    }
                     
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
  }
}
