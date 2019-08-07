using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;

namespace SoftPainter.Serial
{
    public class DataUtils
    {
        public static DeployParam LoadDeployParam(string fileName)
        {
            byte[] rdd = DataUtils.ReadBinary(fileName);
            DeployParam rrs = DataUtils.BytesToObject<DeployParam>(rdd, 0, rdd.Length);
            return rrs;
        }

        public static List<PaintTrack> LoadTracks(string dirname)
        {
            List<PaintTrack> tracks = new List<PaintTrack>();
            DirectoryInfo dirInfo = new DirectoryInfo(dirname);
            FileInfo[] trackFiles = dirInfo.GetFiles();

            foreach (FileInfo trackFile in trackFiles)
            {
                byte[] rdd = DataUtils.ReadBinary(trackFile.FullName);
                PaintTrack rrs = DataUtils.BytesToObject<PaintTrack>(rdd, 0, rdd.Length);
                tracks.Add(rrs);
            }

            return tracks;
        }

        public static PaintTrack LoadTrack(string fileName)
        {
            byte[] rdd = DataUtils.ReadBinary(fileName);
            PaintTrack rrs = DataUtils.BytesToObject<PaintTrack>(rdd, 0, rdd.Length);
            return rrs;
        }

        public static byte[] ObjectToBytes<T>(T instance)
        {
            try
            {
                byte[] array;
                if (instance == null)
                {
                    array = new byte[0];
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream();
                    Serializer.Serialize(memoryStream, instance);
                    array = new byte[memoryStream.Length];
                    memoryStream.Position = 0L;
                    memoryStream.Read(array, 0, array.Length);
                    memoryStream.Dispose();
                }

                return array;

            }
            catch (Exception ex)
            {

                return new byte[0];
            }
        }

        public static T BytesToObject<T>(byte[] bytesData, int offset, int length)
        {
            if (bytesData.Length == 0)
            {
                return default(T);
            }
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(bytesData, 0, bytesData.Length);
                memoryStream.Position = 0L;
                T result = Serializer.Deserialize<T>(memoryStream);
                memoryStream.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static void SaveBinary(string filename, byte[] byteArray)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }

        public static void SafeSaveBinary(string filename, byte[] byteArray)
        {
            string dirPath = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }

        public static byte[] ReadBinary(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
    }
}
