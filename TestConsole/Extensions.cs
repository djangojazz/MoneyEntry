using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestConsole
{
    public static class Extensions
    {
        public static string GetBaseDirectory() => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\.."));
        
        public static string ReadFile(this string location)
        {
            using (var reader = new StreamReader(location))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
