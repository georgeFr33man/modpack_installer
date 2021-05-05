using System;
using System.IO;

namespace ModpackInstaller.Helpers
{
    public class DirectoryHelper
    {
        public static bool CreateDir(string path)
        {
            try {
                Directory.CreateDirectory(path);
            }
            catch (Exception e) {
                ConsoleHelper.Error(e.ToString());
                return false;
            }

            return true;
        }

        public static bool RemoveDir(string path)
        {
            try {
                var files = Directory.GetFiles(path);
                foreach (var file in files) {
                    File.Delete(file);
                }
                Directory.Delete(path);
            }
            catch (Exception e) {
                ConsoleHelper.Error(e.ToString());
                return false;
            }

            return true;
        }

        public static bool RenameDir(string origin, string destination)
        {
            try {
                Directory.Move(origin, destination);
            }
            catch (Exception e) {
                ConsoleHelper.Error(e.ToString());
                return false;
            }

            return true;
        }
    }
}