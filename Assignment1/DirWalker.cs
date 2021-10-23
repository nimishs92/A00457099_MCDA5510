using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
    public class DirWalker
    {
        /// <summary>
        /// Recursivley browse all files in the directory.
        /// </summary>
        /// <param name="path">Path of the root Folder.</param>
        /// <param name="format">Only select files matching the format.</param>
        /// <returns>List of files found</returns>
        public List<string> WalkRecursive(string path, string format)
        {
            List<string> lstFiles = new List<string>();
            string[] dirList = Directory.GetDirectories(path);

            if (dirList == null) return null;

            foreach (var dir in dirList)
            {
                if (Directory.Exists(dir))
                {
                    lstFiles.AddRange(WalkRecursive(dir,format));
                }
            }

            string[] fileList = Directory.GetFiles(path,format);
            lstFiles.AddRange(fileList);

            return lstFiles;
        }

        /// <summary>
        /// Browse all files in the directory. Uses a stack. 
        /// </summary>
        /// <param name="root">Path of the root Folder.</param>
        /// <param name="format">Only select files matching the format.</param>
        /// <returns>List of files found</returns>
        public List<string> WalkStack(string root, string format)
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
                    files = System.IO.Directory.GetFiles(currentDir, format);
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
