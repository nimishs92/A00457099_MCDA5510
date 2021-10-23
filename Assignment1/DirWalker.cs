using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
    public class DirWalker
    {
        public List<string> Walk1(string path)
        {
            List<string> lstFiles = new List<string>();
            string[] dirList = Directory.GetDirectories(path);

            if (dirList == null) return null;

            foreach (var dir in dirList)
            {
                if (Directory.Exists(dir))
                {
                    lstFiles.AddRange(Walk1(dir));
                }
            }

            string[] fileList = Directory.GetFiles(path,"*.csv");
            lstFiles.AddRange(fileList);

            return lstFiles;
        }

        public List<string> Walk(string root)
        {
            List<string> lstFiles = new List<string>();
            Stack<string> dirs = new Stack<string>(20);
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirList = Directory.GetDirectories(currentDir);

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch (Exception ex) { }

                lstFiles.AddRange(files);

                foreach (string dir in subDirList)
                    dirs.Push(dir);
            }
            return lstFiles;
        }
    }
}
