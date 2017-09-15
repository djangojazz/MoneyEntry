using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Controls.BusinessLogic
{
  public sealed class FileIO
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();

    public void OpenLocation(string aLoc)
    {
      if (Directory.Exists(aLoc))
      {
        System.Diagnostics.Process.Start(aLoc);
      }
      else
      {
        MessageBox.Show("Directory does not exist");
      }
    }
  }
}
