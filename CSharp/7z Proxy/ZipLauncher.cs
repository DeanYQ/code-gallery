using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _7zProxy
{
    public class ZipLauncher
    {
        private const string DefaultInstallPath = @"C:\Program Files\7-Zip\7z.exe";
        private const string X86InstallPath = @"C:\Program Files (x86)\7-Zip\7z.exe";
        private const string X64InstallPath = @"C:\Program Files\7-Zip\7z.exe";

        public Zip7zProxy Launch()
        {
            if (File.Exists(DefaultInstallPath))
            {
                InstallPath = DefaultInstallPath;
            }
            else if (File.Exists(X86InstallPath))
            {
                InstallPath = X86InstallPath;
            }
            else if (File.Exists(X64InstallPath))
            {
                InstallPath = X64InstallPath;
            }

            if (string.IsNullOrWhiteSpace(InstallPath))
                return null;

            return new Zip7zProxy(InstallPath);
        }

        public string InstallPath { get; private set; }
    }
}
