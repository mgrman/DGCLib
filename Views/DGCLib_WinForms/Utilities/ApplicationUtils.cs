using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DGCLib_WinForms.Utilities
{
    public static class ApplicationUtils
    {
        private static string _localAppDataPath = "";

        public static string LocalAppDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_localAppDataPath))
                {
                    var temp = Application.LocalUserAppDataPath;
                    if (Directory.GetFiles(temp).Length == 0)
                        Directory.Delete(temp);
                    var array = temp.Split('\\').Reverse().Skip(1).Reverse().ToArray();
                    _localAppDataPath = String.Join("\\", array);
                }
                return _localAppDataPath;
            }
        }
    }
}