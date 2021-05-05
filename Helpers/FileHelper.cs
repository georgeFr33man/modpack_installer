using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ModpackInstaller.Helpers
{
    public class FileHelper
    {
        public static async Task DownloadFile(string address, string destination)
        {
            try {
                var client = new WebClient();
                var uri = new Uri(address);
                client.DownloadProgressChanged += DownloadProgressCallback;
                client.DownloadFileCompleted += DownloadCompleted;
                await client.DownloadFileTaskAsync(uri, destination);
            }
            catch (Exception e) {
                ConsoleHelper.Error("Could not download file.");
            }
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            var received = e.BytesReceived;
            if (received % 10 == 0) {
                ConsoleHelper.Info(string.Format("Downloaded {0} of {1} bytes. {2} % complete...",
                    received,
                    e.TotalBytesToReceive,
                    e.ProgressPercentage));
            }
        }

        private static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
                ConsoleHelper.Success("Download complete");
        }
        
        public static Task<int> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<int>();
            var process = new Process
            {
                StartInfo = { FileName = fileName },
                EnableRaisingEvents = true
            };
            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };
            process.Start();
            return tcs.Task;
        }
    }
}