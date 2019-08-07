using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ProtoBuf;

namespace SoftPainter.Serial
{
    /// <summary>
    /// 轨迹库(不存储)
    /// </summary>
    public class TrackLibrary
    {
        /// <summary>
        /// 存储所有轨迹的字典，由JointKey唯一确定一种轨迹
        /// 设计思路：通过JointKey快速获取对应的轨迹
        /// </summary>
        public Dictionary<JointKey, PaintTrack> Library { get; set; }
        public Dictionary<string, DeployParam> DeployParamSearch { get; set; }

        public TrackLibrary(List<PaintTrack> library)
        {
            Library = LoadLibraryDictionary(library);
            DeployParamSearch = LoadDeployParamDictionary();
        }

        public Dictionary<JointKey, PaintTrack> LoadLibraryDictionary(List<PaintTrack> library)
        {
            Dictionary<JointKey, PaintTrack> dictionary = new Dictionary<JointKey, PaintTrack>();
            foreach (var track in library)
            {
                try
                {
                    dictionary.Add(track.GUID, track);
                }
                catch (Exception err)
                {
                    Trace.TraceError(err.ToString());
                }
            }
            return dictionary;
        }

        public Dictionary<string, DeployParam> LoadDeployParamDictionary()
        {
            Dictionary<string, DeployParam> dictionary = new Dictionary<string, DeployParam>();

            DirectoryInfo rootDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library-model-match"));
            DirectoryInfo[] subdirs = rootDir.GetDirectories();
            foreach(DirectoryInfo dir in subdirs)
            {
                string filename = Path.Combine(dir.FullName, dir.Name + ".ini");
                if(File.Exists(filename))
                {
                    DeployParam deployParam = DataUtils.LoadDeployParam(filename);
                    dictionary.Add(dir.Name, deployParam);
                }
            }

            return dictionary;
        }

        public static bool SaveDeployParam(string Type, LocationModeEnum Location_Mode, TimesMode Times_Mode, int MinZ, int MaxZ)
        {
            try
            {
                DeployParam deployParam = new DeployParam(Type, Location_Mode, Times_Mode, MinZ, MaxZ);
                byte[] data = DataUtils.ObjectToBytes(deployParam);
                string filePath = MatchIniFile(AppDomain.CurrentDomain.BaseDirectory, Type);
                DataUtils.SafeSaveBinary(filePath, data);
                return true;
            }
            catch(Exception err)
            {
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        public static string OriginDestFile(string rootPath, string type, Face face, TimesMode times)
        {
            return Path.Combine(rootPath, string.Format("library-model-origin\\{0}\\{0}.{1}.{2}.origin.pcd", type, face.ToString().ToLower(), times.ToString().ToLower()));
        }

        public static string MatchDestFile(string rootPath, string type, Face face, TimesMode times)
        {
            return Path.Combine(rootPath, string.Format("library-model-match\\{0}\\{0}.{1}.{2}.match.pcd", type, face.ToString().ToLower(), times.ToString().ToLower()));
        }

        public static string MatchIniFile(string rootPath, string type)
        {
            return Path.Combine(rootPath, string.Format("library-model-match\\{0}\\{0}.ini", type));
        }

        public static string TrackDestFile(string rootPath, string type, Face face, TimesMode times)
        {
            return Path.Combine(rootPath, string.Format("library-model-track\\{0}\\{0}.{1}.{2}.track.ply", type, face.ToString().ToLower(), times.ToString().ToLower()));
        }


    }
}
