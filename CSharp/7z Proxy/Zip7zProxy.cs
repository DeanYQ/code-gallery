using System.Diagnostics;
using System.IO;

namespace _7zProxy
{
    public class Zip7zProxy
    {
        private string _7zPath = string.Empty;

        public Zip7zProxy(string progPath)
        {
            _7zPath = progPath;
        }

        public void Unzip(string zipFilePath, string dirOut)
        {
            if (!File.Exists(zipFilePath))
                return;

            var dir = Path.GetDirectoryName(dirOut);
            if (!Directory.Exists(dir))
                return;

            using (var process = new Process())
            {
                process.StartInfo.FileName = _7zPath;
                process.StartInfo.Arguments = string.Format(@"x {0} -o{1}", zipFilePath, dirOut);
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
            }
        }

        public void Zip(string input , string outPut)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = _7zPath;
                process.StartInfo.Arguments = string.Format(@"a {0} {1}", outPut, input);
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
            }
        }
    }
}
