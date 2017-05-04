using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using log4net;
using System.Reflection;

namespace StubManager
{
    public class StubManager
    {
        private readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string basePath_ = string.Empty;
        private string localPath_ = string.Empty;

        public StubObject StubObj { get; set; }

        public string StubFilePath
        {
            get
            {
                return Path.Combine(localPath_, "stub.json");
            }
        }

        public StubManager(string basePath, string localPath)
        {
            if (Directory.Exists(basePath) && Directory.Exists(localPath))
            {
                basePath_ = basePath;
                localPath_ = localPath;
                StubObj = new StubObject();
            }
            throw new ArgumentException();
        }

        public async void GenerateStubData()
        {
            var baseDir = new DirectoryInfo(basePath_);
            var localDir = new DirectoryInfo(localPath_);
            await GenerateStubDataInternal(baseDir, localDir);
        }

        public Task GenerateStubDataInternal(DirectoryInfo baseDir, DirectoryInfo localDir)
        {
            return Task.Run(() =>
            {
                var basefiles = baseDir.GetFiles();
                var localfiles = localDir.GetFiles();

                foreach (var lf in localfiles)
                {
                    var stub = StubItem.FromFile(lf);
                    // exclude
                    if (basefiles.All(i => i.Name != lf.Name))
                    {
                        stub.IsIgnored = true;
                    }
                    // have
                    StubObj.AddStubItem(stub);
                }

                var baseDirectories = baseDir.GetDirectories();
                var localDirectories = localDir.GetDirectories();
                var tasks = new List<Task>();
                foreach (var cd in localDirectories)
                {
                    var stub = StubItem.FromDirectory(cd);
                    var rd = baseDirectories.FirstOrDefault(i => i.Name == cd.Name);
                    if (rd == null)
                    {
                        stub.IsIgnored = true;
                        continue;
                    }

                    StubObj.AddStubItem(stub);

                    var task = GenerateStubDataInternal(rd, cd);
                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(StubObj, Formatting.Indented);
                if (string.IsNullOrEmpty(json))
                    return;

                File.WriteAllText(StubFilePath, json);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        public bool Load()
        {
            try
            {
                if (File.Exists(StubFilePath))
                {
                    var json = File.ReadAllText(StubFilePath);
                    StubObj = JsonConvert.DeserializeObject<StubObject>(json);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
