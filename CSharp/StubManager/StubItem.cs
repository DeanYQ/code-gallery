using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace StubManager
{
    public class StubItem
    {
        public byte[] Hash { get; set; }

        public string FullPath { get; set; }

        /// <summary>
        /// Include extension
        /// </summary>
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsFile { get; set; }

        public bool IsIgnored { get; set; }

        public static StubItem FromFile(FileInfo file)
        {
            var stubItem = new StubItem();
            stubItem.FullPath = file.FullName;
            stubItem.IsFile = true;
            stubItem.Name = file.Name;
            stubItem.Timestamp = DateTime.Now;
            using (var stream = file.OpenRead())
            {
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    stubItem.Hash = hash.ComputeHash(stream);
                }
            }
            return stubItem;
        }

        public static StubItem FromDirectory(DirectoryInfo dir)
        {
            var stubItem = new StubItem();
            stubItem.FullPath = dir.FullName;
            stubItem.IsFile = false;
            stubItem.Name = dir.Name;
            stubItem.Timestamp = DateTime.Now;
            return stubItem;
        }
    }
}
