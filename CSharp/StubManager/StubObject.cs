using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StubManager
{
    public class StubObject
    {
        public StubObject()
        {
            Items = new List<StubItem>();
        }

        public List<StubItem> Items { get; set; }

        public void AddStubItem(StubItem item)
        {
            if (item == null)
                return;

            if (Items.Exists(i => i.FullPath == item.FullPath))
                return;

            Items.Add(item);
        }

        public void AddFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
            }
        }

        public void RemoveFile()
        {

        }

        public void AddDirectory()
        {

        }

        public void RemoveDirectory()
        {

        }

    }
}
