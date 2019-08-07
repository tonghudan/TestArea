using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftPainter.History.Util
{
    public class FileHandler
    {
        /// <summary>
        /// 将类obj序列化保存到filePath文件中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SaveObjec<T>(string filePath, T obj)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
                return true;
            }
            catch(Exception e)
            {
                Trace.TraceError(e.ToString());
                return false;
            }
            //finally
            //{
            //    fs.Close();
            //}
        }

        /// <summary>
        /// 从filePath文件中反序列化加载出类obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadObject<T>(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                T obj = (T)bf.Deserialize(fs);
                fs.Close();
                return obj;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return default(T);
            }
            //finally
            //{
            //    fs.Close();
            //}
        }

        public static string[] KLAESParse(string path)
        {
            List<string> paths = new List<string>();
            StreamReader sr = new StreamReader(path);
            string para = sr.ReadToEnd();

            string[] subFiles = para.Replace("\r\n\r\n", "@").Split('@');

            string dir = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path).Replace(".txt", "");
            string newDir = Path.Combine(dir, file);
            Directory.CreateDirectory(newDir);
            int id = 0;
            foreach(string content in subFiles)
            {
                string tpath = Path.Combine(newDir, String.Format("{0}.txt", id++));
                WriteAll(tpath, content + "\n");
                paths.Add(tpath);
            }

            return paths.ToArray();
        }

        public static void WriteLines(string path, string[] lines)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                //开始写入
                foreach (string line in lines)
                    sw.WriteLine(line);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

        public static void WriteAll(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content);
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

        public static void WriteAll(string path, byte[] content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fs))
            {
                sw.Write(content);
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 删除文件夹下的所有文件，若文件夹不存在则创建一个空文件夹
        /// </summary>
        /// <param name="srcPath"></param>
        public static void DeleteDir(string srcPath)
        {
            try
            {
                if(!Directory.Exists(srcPath))
                {
                    Directory.CreateDirectory(srcPath);
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CopyDir(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
                return;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                File.Copy(file.FullName, Path.Combine(destDir, Path.GetFileName(file.FullName)), true);
            }
        }

        public static string CopyFile(string sourcrFile, string destDir)
        {
            if (!File.Exists(sourcrFile))
                return null;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string destFile = Path.Combine(destDir, Path.GetFileName(sourcrFile));
            File.Copy(sourcrFile, destFile, true);
            return destFile;
        }

        public static string GetSingleFile(string sourceDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return null;
            FileInfo[] files = dir.GetFiles();
            if (files.Length != 1)
                return null;
            return files[0].FullName;
        }

        public static void CopyClassifiedImage(string sourceFile, string destRootDir, string type)
        {
            if (!File.Exists(sourceFile))
                return;
            string destDir = Path.Combine(destRootDir, string.IsNullOrEmpty(type) ? "" : type);
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            DateTime now = DateTime.Now;
            string destFile = Path.Combine(destDir, string.Format("{0}_{1:D2}_{2:D2}_{3:D2}_{4:D2}_{5:D2}_{6:D3}_{7}.jpeg", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, now.TimeOfDay.Ticks));
            File.Copy(sourceFile, destFile, true);
        }
    }
}
