using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SoftPainter.History.Util
{
    public class FileIO
    {
        //split teil file
        public void SplitTeilFile(string file, string saveDirectoryPath)
        {
            List<string> lst = new List<string>();
            using (System.IO.StreamReader sw = new System.IO.StreamReader(file))
            {
                while (!sw.EndOfStream)
                {
                    string line = sw.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    lst.Add(line);
                }
            }

            int index = lst.IndexOf("[TEIL]");
            List<string> prefex = lst.GetRange(0, index);
            List<List<string>> teilist = this.FindTEIL(lst);
            this.WriteTEIL(saveDirectoryPath + "\\", teilist, prefex);
        }

        private List<List<string>> FindTEIL(List<string> lst)
        {
            List<List<string>> teilist = new List<List<string>>();

            List<string> teil = null;
            foreach (string str in lst)
            {
                if(str.StartsWith("[ENDE] "))
                    break;

                if (str.StartsWith("[TEIL]"))
                {
                    teil = new List<string>();
                    teilist.Add(teil);
                }

                if (teil != null)
                    teil.Add(str);
            }

            return teilist;
        }

        private void WriteTEIL(string dictory, List<List<string>> teilist, List<string> prefex)
        {
            foreach (List<string> teil in teilist)
            {
                int index = -1;
                for (int i = 0; i < teil.Count; i++)
                {
                    index = i;
                    if (teil[i].StartsWith("BARCODE="))
                        break;
                }

                if (-1 == index)
                    continue;

                string name = teil[index];
                index = name.IndexOf('=');
                if (-1 == index)
                    continue;

                name = name.Substring(index + 1, name.Length - index - 1).Trim();

                int lCount = 0;
                int eCount = 0;
                for (int i = 0; i < teil.Count; i++)
                {
                    if(teil[i].StartsWith("[TEIL]"))
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dictory + name + ".txt"))
                        {
                            foreach (string str in prefex)
                                sw.WriteLine(str);
                            sw.WriteLine(teil[i]);
                            i++;

                            while (i < teil.Count && !teil[i].StartsWith("["))
                            {
                                sw.WriteLine(teil[i]);
                                i++;
                            }
                            i--;
                        }
                    }
                    else if (teil[i].StartsWith("[LBOHR]"))
                    {
                        lCount++;
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dictory + name + "_LBOHR" + lCount.ToString() + ".txt"))
                        {
                            sw.WriteLine(teil[i]);
                            i++;

                            while (i < teil.Count && !teil[i].StartsWith("["))
                            {
                                sw.WriteLine(teil[i]);
                                i++;
                            }
                            i--;
                        }
                    }
                    else if (teil[i].StartsWith("[EBOHR]"))
                    {
                        eCount++;
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dictory + name + "_EBOHR" + eCount.ToString() + ".txt"))
                        {
                            sw.WriteLine(teil[i]);
                            i++;

                            while (i < teil.Count && !teil[i].StartsWith("["))
                            {
                                sw.WriteLine(teil[i]);
                                i++;
                            }
                            i--;
                        }
                    }
                }
            }
        }

        //ensure path
        public static bool EnsurePath(string folderPath)
        {
            try
            {
                if (System.IO.Directory.Exists(folderPath))
                    return true;
                System.IO.Directory.CreateDirectory(folderPath);
                return System.IO.Directory.Exists(folderPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(true, e.Message);
                return false;
            }
        }

        public static string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static bool SafeFileCopy(string sourcefile, string destfile)
        {
            try
            {
                if (!File.Exists(sourcefile)) return false;
                string dirPath = Path.GetDirectoryName(destfile);
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                File.Copy(sourcefile, destfile, true);
                return true;
            }
            catch (Exception err)
            {
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        //读取加工文件, 以字符串格式存储数值(以[]中的项目名称为Key, 如[ETIL]/[LBOHR]/[EBOHR]..)
        public static Dictionary<string, Dictionary<string, string>> ReadProcessFile(string file)
        {
            Dictionary<string, Dictionary<string, string>> contents = new Dictionary<string, Dictionary<string, string>>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
            {
                Dictionary<string, string> item = null;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("["))
                    {
                        if (!contents.ContainsKey(line))
                        {
                            item = new Dictionary<string, string>();
                            contents[line] = item;
                        }
                        else
                            item = contents[line]; //多个LBOHR或多个EBOHR的情况
                    }
                    else if (null != item)
                    {
                        int index = line.IndexOf('=');
                        if (-1 == index)
                            continue;

                        string key = line.Substring(0, index);
                        if (!item.ContainsKey(key))
                            item[key] = line.Substring(index + 1, line.Length - index - 1);
                        else
                            item[key] = item[key] + ",\n" + line.Substring(index + 1, line.Length - index - 1);///?///为什么以",\n"分割
                    }
                }
            }

            return contents;
        }

        public static int ReadProcessFile(string file, out Dictionary<string, string> kophDict, out Dictionary<string, string> teilDict, out List<Dictionary<string, string>> lBohrDict, out List<Dictionary<string, string>> eBohrDict)
        {
            kophDict = new Dictionary<string, string>();
            teilDict = new Dictionary<string, string>();
            lBohrDict = new List<Dictionary<string, string>>();
            eBohrDict = new List<Dictionary<string, string>>();

            //Dictionary<string, Dictionary<string, string>> contents = new Dictionary<string, Dictionary<string, string>>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
            {
                Dictionary<string, string> item = null;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("["))
                    {
                        if (line.StartsWith("[KOPF]"))
                            item = kophDict;
                        else if (line.StartsWith("[TEIL]"))
                            item = teilDict;
                        else if (line.StartsWith("[LBOHR]"))
                        {
                            item = new Dictionary<string, string>();
                            lBohrDict.Add(item);
                        }
                        else if (line.StartsWith("[EBOHR]"))
                        {
                            item = new Dictionary<string, string>();
                            eBohrDict.Add(item);
                        }
                    }
                    else if (null != item)
                    {
                        int index = line.IndexOf('=');
                        if (-1 == index)
                            continue;

                        string key = line.Substring(0, index);
                        if (!item.ContainsKey(key))
                            item[key] = line.Substring(index + 1, line.Length - index - 1);
                        else
                            item[key] = item[key] + ",\n" + line.Substring(index + 1, line.Length - index - 1);
                    }
                }
            }

            return Math.Max(lBohrDict.Count, eBohrDict.Count);
        }

        //读/写workcode文件
    }
}
