using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace ProjectBuilder
{
    public class ProjectFileParser
    {
        public ProjectFileParser()
        {

        }

        private string FullPath { get; set; }

        private XDocument Doc { get; set; }

        public void Load(string path)
        {
            FullPath = Path.GetFullPath(path);
            Doc = XDocument.Load(FullPath);
        }

        public ProjectFileInfo GetProjFileInfo()
        {
            var wpBak = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetDirectoryName(FullPath);

            var file = new ProjectFileInfo();
            var refs = GetReferences();
            var projRefs = GetProjectReferences();

            file.References.AddRange(ConvertXElementsToRefs(refs));
            file.ProjReferences.AddRange(ConvertXElementsToProjRefs(projRefs));
            file.AssemblyName = GetAssemblyName();
            file.Complies.AddRange(GetComplieItems());
            file.FileFullPath = FullPath;
            file.FileName = Path.GetFileName(FullPath);

            Environment.CurrentDirectory = wpBak;
            return file;
        }

        private string GetAssemblyName()
        {
            var elements = GetXElements(Doc.Root, "AssemblyName");
            if (elements != null)
            {
                var element = elements.FirstOrDefault();
                if (element != null)
                {
                    return element.Value;
                }
            }
            return string.Empty;
        }

        private List<string> GetComplieItems()
        {
            var lst = new List<string>();
            if (Doc != null)
            {
                var compiles = GetXElements(Doc.Root, "Compile");
                foreach (var item in compiles)
                {
                    var include = item.Attribute("Include");
                    if (include != null)
                    {
                        lst.Add(Path.GetFullPath(include.Value));
                    }
                }
            }
            return lst;
        }

        private List<XElement> GetReferences()
        {
            var lst = new List<XElement>();
            if (Doc != null)
            {
                lst.AddRange(GetXElements(Doc.Root, "Reference"));
            }
            return lst;
        }

        private List<XElement> GetProjectReferences()
        {
            var lst = new List<XElement>();
            if (Doc != null)
            {
                lst.AddRange(GetXElements(Doc.Root, "ProjectReference"));
            }
            return lst;
        }

        private List<XElement> GetXElements(XElement root, string name)
        {
            var list = new List<XElement>();
            if (root != null)
            {
                var elements = root.Elements().Where(i => i.Name.LocalName == name);
                list.AddRange(elements);
                foreach (var sub in root.Elements())
                {
                    list.AddRange(GetXElements(sub, name));
                }

            }
            return list;
        }

        private List<Reference> ConvertXElementsToRefs(List<XElement> elements)
        {
            var lst = new List<Reference>();
            if (elements != null)
            {
                var refs = GetReferences();
                foreach (var r in refs)
                {
                    var info = new Reference();
                    var inc = r.Attribute("Include");
                    if (inc != null)
                    {
                        info.Include = inc.Value;
                    }
                    var hint = r.Elements().FirstOrDefault(i => i.Name.LocalName == "HintPath");
                    if (hint != null)
                    {
                        info.HintPath = hint.Value;
                        info.FullPath = Path.GetFullPath(info.HintPath);
                    }
                    var priv = r.Elements().FirstOrDefault(i => i.Name.LocalName == "Private");
                    if (priv != null)
                    {
                        info.IsPrivate = (priv.Value == "True");
                    }
                    lst.Add(info);
                }
            }
            return lst;
        }

        private List<ProjReference> ConvertXElementsToProjRefs(List<XElement> elements)
        {
            var lst = new List<ProjReference>();
            if (elements != null)
            {
                var refs = GetProjectReferences();
                foreach (var r in refs)
                {
                    var info = new ProjReference();
                    var inc = r.Attribute("Include");
                    if (inc != null)
                    {
                        info.Include = inc.Value;
                    }
                    var proj = r.Elements().FirstOrDefault(i => i.Name.LocalName == "project");
                    if (proj != null)
                    {
                        info.ProjectId = Guid.Parse(proj.Value);
                    }
                    var name = r.Elements().FirstOrDefault(i => i.Name.LocalName == "Name");
                    if (name != null)
                    {
                        info.Name = name.Value;
                    }
                    info.FullPath = Path.GetFullPath(info.Include);
                    lst.Add(info);
                }
            }
            return lst;
        }
    }
}
