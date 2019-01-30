using System;
using System.IO;

namespace MoneyEntry.DataAccess.EFCore
{
    public static class Extensions
    {
        public static string BaseDirectory { get => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..")); }
        
        public static string ReadFile(this string location)
        {
            using (var reader = new StreamReader(location))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
