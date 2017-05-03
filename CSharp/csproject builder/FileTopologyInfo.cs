using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBuilder
{
    public class FileTopologyInfo
    {
        public FileTopologyInfo()
        {
            Descendants = new List<ProjectFileInfo>();
        }

        public ProjectFileInfo ProjInfo { get; set; }

        public int PathPoint { get; set; }

        public List<ProjectFileInfo> Descendants { get; set; }
    }
}
