using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBuilder
{
    public class ProjectFileInfo
    {
        public ProjectFileInfo()
        {
            FileName = string.Empty;
            AssemblyName = string.Empty;
            Complies = new List<string>();
            References = new List<Reference>();
            ProjReferences = new List<ProjReference>();
        }

        public string FileName { get; set; }

        public string FileFullPath { get; set; }

        public string AssemblyName { get; set; }

        public List<string> Complies { get; set; }

        public List<Reference> References { get; set; }

        public List<ProjReference> ProjReferences { get; set; }
    }

    public class Reference
    {
        /// <summary>
        /// dll's name
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        /// dll's relative path.
        /// </summary>
        public string HintPath { get; set; }

        public bool? IsPrivate { get; set; }

        public string FullPath { get; set; }
    }

    public class ProjReference
    {
        /// <summary>
        /// project guid.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// dll relative path.
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        /// Dll's name
        /// </summary>
        public string Name { get; set; }

        public string FullPath { get; set; }
    }

}
