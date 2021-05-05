using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ModpackInstaller.Helpers;
using ModpackInstaller.Schemas;


namespace ModpackInstaller.Providers
{
    public class PathProvider
    {
        public PathProvider()
        {
            SystemUserName = Environment.UserName;
            DotMinecraftPath = $"C:\\Users\\{SystemUserName}\\AppData\\Roaming\\.minecraft\\";
            VersionsFolderPath = DotMinecraftPath + "versions";
            Hash = CreateHash();
            ModsFolderPath = DotMinecraftPath + "mods";
            OldModsFolderPath = ModsFolderPath + "-" + Hash;
            ModsFile = GetModsFilePath();
            ModsData = GetModsData();
        }

        public string ModsFolderPath { get; }
        public string Hash { get; }
        public string DotMinecraftPath { get; }
        public string OldModsFolderPath { get; }
        public string VersionsFolderPath { get; }
        public MainSchema ModsData { get; }
        private string ModsFile { get; }
        private string SystemUserName { get; }


        private static string CreateHash(string value = null)
        {
            value = value ?? DateTime.UtcNow.ToString(CultureInfo.CurrentCulture);
            var md5Hash = MD5.Create();
            var hashedDate = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToString(hashedDate).Replace("-", string.Empty);
        }

        public MainSchema GetModsData()
        {
            var jsonString = File.ReadAllText(ModsFile);
            return JsonSerializer.Deserialize<MainSchema>(jsonString);
        }

        public string GetModsFilePath()
        {
            var path = "mods.json";
            if (File.Exists(path)) {
                try {
                    File.ReadAllText(path);
                }
                catch (Exception) {
                    ConsoleHelper.Error("Could not load mods.json file.");
                }
            } else {
                throw new FileNotFoundException("Could not find a mods.json file.");
            }

            return path;
        }
    }
}