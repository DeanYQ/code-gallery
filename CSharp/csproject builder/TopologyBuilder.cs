using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjectBuilder
{
    public class TopologyBuilder
    {
        public List<FileTopologyInfo> Build(DirectoryInfo directory)
        {
            var list = new List<FileTopologyInfo>();
            if (directory != null)
            {
                // currently just for VC#.
                var files = directory.GetFiles("*.csproj", SearchOption.AllDirectories);
                if (files != null)
                {
                    var topologies = Build(files.Select(i => i.FullName));
                    list.AddRange(topologies);
                }
            }
            return list;
        }

        /// <summary>
        /// Build topology.
        /// </summary>
        /// <param name="projectFiles">project file full paths.</param>
        /// <returns></returns>
        public List<FileTopologyInfo> Build(IEnumerable<string> projectFiles)
        {
            var projects = ParseFiles(projectFiles);
            var topologis = ParseProjects(projects);
            CalculateTopologyPoint(topologis);
            return topologis;
        }

        public List<FileTopologyInfo> BuildFiles(DirectoryInfo directory, IEnumerable<string> csfiles)
        {
            var result = new List<FileTopologyInfo>();
            var topologyFiles = Build(directory);
            foreach (var file in csfiles)
            {
                var pfiles = topologyFiles.Where(i => i.ProjInfo.Complies.Contains(file));
                foreach (var pf in pfiles)
                {
                    if (!result.Contains(pf))
                    {
                        result.Add(pf);
                    }
                }
            }
            return result;
        }

        private List<ProjectFileInfo> ParseFiles(IEnumerable<string> projectFiles)
        {
            var projs = new List<ProjectFileInfo>();
            var parser = new ProjectFileParser();
            foreach (var file in projectFiles)
            {
                parser.Load(file);
                var projInfo = parser.GetProjFileInfo();
                // filter system, global, microsoft assemblies.
                projInfo.References.RemoveAll(i => string.IsNullOrWhiteSpace(i.HintPath));
                projs.Add(projInfo);
            }
            return projs;
        }

        private List<FileTopologyInfo> ParseProjects(IEnumerable<ProjectFileInfo> projects)
        {
            var topoInfos = new List<FileTopologyInfo>();
            foreach (var proj in projects)
            {
                var topo = new FileTopologyInfo();
                topo.ProjInfo = proj;
                foreach (var refer in proj.References)
                {
                    var p = projects.FirstOrDefault(i => i.AssemblyName == Path.GetFileNameWithoutExtension(refer.FullPath));
                    if (p != null)
                    {
                        topo.Descendants.Add(p);
                    }
                }

                foreach (var prefer in proj.ProjReferences)
                {
                    var p = projects.FirstOrDefault(i => prefer.Name == Path.GetFileNameWithoutExtension(i.FileFullPath));
                    if (p != null)
                    {
                        topo.Descendants.Add(p);
                    }
                }
                topoInfos.Add(topo);
            }
            return topoInfos;
        }

        private void CalculateTopologyPoint(IEnumerable<FileTopologyInfo> topologies)
        {
            var leafNodes = topologies.Where(i => i.Descendants.Count <= 0).ToList();
            foreach (var leaf in leafNodes)
            {
                UpdateParentPoint(leaf, topologies);
            }
        }

        private void UpdateParentPoint(FileTopologyInfo child, IEnumerable<FileTopologyInfo> topologies)
        {
            var basePoint = child.PathPoint;
            foreach (var item in topologies)
            {
                var first = item.Descendants.FirstOrDefault(i => i.FileFullPath == child.ProjInfo.FileFullPath);
                if (first != null)
                {
                    var newPoint = basePoint + 1;
                    if (item.PathPoint < newPoint)
                    {
                        item.PathPoint = newPoint;
                    }
                    UpdateParentPoint(item, topologies);
                }
            }
        }
    }
}
