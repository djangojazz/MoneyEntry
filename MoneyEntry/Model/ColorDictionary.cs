using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoneyEntry.Model
{
  internal static class ColorDictionary
  {
    internal static Dictionary<byte, Brush> Color
    {
      get => new Dictionary<byte, Brush>
      {
        { 1, Brushes.BlanchedAlmond },
        { 2, Brushes.BlueViolet },
        { 3, Brushes.Goldenrod },
        { 4, Brushes.Aqua },
        { 5, Brushes.Purple },
        { 6, Brushes.Azure },
        { 7, Brushes.Blue },
        { 8, Brushes.Lime },
        { 9, Brushes.Orange },
        { 10, Brushes.Red },
        { 11, Brushes.Beige },
        { 12, Brushes.DarkMagenta },
        { 13, Brushes.Sienna },
        { 14, Brushes.Brown },
        { 15, Brushes.MediumVioletRed },
        { 16, Brushes.LightCyan },
        { 17, Brushes.DarkGray },
        { 18, Brushes.DarkKhaki },
        { 19, Brushes.MediumOrchid },
        { 20, Brushes.DarkBlue },
        { 21, Brushes.LightSlateGray },
        { 22, Brushes.MediumSlateBlue },
        { 23, Brushes.Navy },
        { 24, Brushes.DarkCyan },
        { 25, Brushes.MediumSeaGreen },
        { 26, Brushes.SandyBrown },
        { 27, Brushes.LawnGreen },
        { 28, Brushes.SeaShell },
        { 29, Brushes.OrangeRed },
        { 30, Brushes.MediumPurple },
        { 31, Brushes.DarkOliveGreen },
        { 32, Brushes.OliveDrab },
        { 33, Brushes.Gold },
        { 34, Brushes.CornflowerBlue },
        { 35, Brushes.Aquamarine },
        { 36, Brushes.IndianRed },
        { 37, Brushes.PaleVioletRed },
        { 38, Brushes.BurlyWood },
        { 39, Brushes.CadetBlue },
        { 40, Brushes.MediumSpringGreen }
      };  
    }
  }
}
