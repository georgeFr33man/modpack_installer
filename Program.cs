using System;
using System.IO;
using System.Threading.Tasks;
using ModpackInstaller.Helpers;
using ModpackInstaller.Providers;

namespace ModpackInstaller
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var pathProvider = new PathProvider();
            if (Directory.Exists(pathProvider.DotMinecraftPath) == false) {
                ConsoleHelper.Error(".minecraft not found in: " + pathProvider.DotMinecraftPath);
            } else {
                if (PerformForgeCheck(pathProvider) == false)
                    await DownloadAndInstallForge(pathProvider);
                if (PerformForgeCheck(pathProvider) == false) {
                    ConsoleHelper.Error("Forge not installed. Aborted.");
                    Console.ReadLine();
                    return;
                }

                ConsoleHelper.Success("Forge installed.");
                CreateModsFolder(pathProvider);
                await DownloadMods(pathProvider);
                await DownloadShaders(pathProvider);
            }
            ConsoleHelper.Success("Modpack successfully installed.");
            Console.ReadLine();
        }

        private static async Task DownloadAndInstallForge(PathProvider pathProvider)
        {
            var path = pathProvider.DotMinecraftPath + "tmp";
            var destination = path + "\\forge-tmp.jar";
            var address = pathProvider.ModsData.Forge.Url;
            if (DirectoryHelper.CreateDir(path) == false) {
                ConsoleHelper.Error("Forge cannot been downloaded. Aborted");
                return;
            }
            ConsoleHelper.SuperInfo($"Downloading {pathProvider.ModsData.Forge.Name}");
            await FileHelper.DownloadFile(address, destination);
            ConsoleHelper.SuperInfo($"Opening {pathProvider.ModsData.Forge.Name} installer");
            await FileHelper.RunProcessAsync(destination);
            if (DirectoryHelper.RemoveDir(path) == false) {
                ConsoleHelper.Error($"Failed removing temporary folder in: {path}");
            }
        }

        private static async Task DownloadMods(PathProvider pathProvider)
        {
            foreach (var mod in pathProvider.ModsData.Mods) {
                ConsoleHelper.SuperInfo($"Downloading {mod.Name}");
                var destination = pathProvider.ModsFolderPath + "\\" + mod.PackageName;
                var fileDownload = FileHelper.DownloadFile(mod.Url, destination);
                await fileDownload;
            }
        }
        
        private static async Task DownloadShaders(PathProvider pathProvider)
        {
            var path = pathProvider.DotMinecraftPath + "shaderpacks\\";
            var destination = path + pathProvider.ModsData.Shaders.PackageName;
            var address = pathProvider.ModsData.Shaders.Url;
            if (DirectoryHelper.CreateDir(path) == false) {
                ConsoleHelper.Error("Shaders cannot been downloaded. Failed");
                return;
            }
            ConsoleHelper.SuperInfo($"Downloading {pathProvider.ModsData.Shaders.Name}");
            await FileHelper.DownloadFile(address, destination);
        }
        private static bool PerformForgeCheck(PathProvider pathProvider)
        {
            var path = pathProvider.VersionsFolderPath + "\\" + pathProvider.ModsData.MinecraftVersion + "-forge-" +
                       pathProvider.ModsData.Forge.Version;
            return Directory.Exists(path);
        }

        private static void CreateModsFolder(PathProvider pathProvider)
        {
            var mfp = pathProvider.ModsFolderPath;
            var omfp = pathProvider.OldModsFolderPath;
            if (Directory.Exists(pathProvider.ModsFolderPath) == false ||
                Directory.GetFiles(pathProvider.ModsFolderPath).Length > 0) {
                if (Directory.Exists(pathProvider.ModsFolderPath)) {
                    ConsoleHelper.Warn("Mods folder found in .minecraft folder, renaming.");
                    if (DirectoryHelper.RenameDir(mfp, omfp) == false)
                        ConsoleHelper.Error("Aborting...");
                }

                ConsoleHelper.Info("Creating mods folder in: " + mfp);
                if (DirectoryHelper.CreateDir(mfp)) return;
                ConsoleHelper.Warn("Restoring changes...");
                if (DirectoryHelper.RenameDir(omfp, mfp) == false)
                    ConsoleHelper.Warn("Restoring changes failed. Your mods folder has been renamed to: " +
                                       mfp + "-" + pathProvider.Hash);
            } else {
                ConsoleHelper.Info("Mods folder is already empty. Skipping.");
            }
        }
    }
}