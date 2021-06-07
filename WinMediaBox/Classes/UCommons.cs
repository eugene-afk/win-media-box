using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace WinMediaBox.Classes
{
    public static class UCommons
    {
        public static string moviesBasePath;
        public static string postersBasePath;
        public static string ipTVPlayerPath;
        private static readonly string[] imageExtensions = { ".JPG", ".JPEG", ".PNG" };

        public static void Init()
        {
            InitLogger();
            CheckDataExist();
            InitCorePathes();
        }

        private static void InitCorePathes()
        {
            CorePathes item = JsonConvert.DeserializeObject<CorePathes>(File.ReadAllText("data/corePathes.json"));
            moviesBasePath = item.moviesBasePath;
            postersBasePath = item.postersBasePath;
            ipTVPlayerPath = item.ipTVPlayerPath;
        }

        private static void InitLogger()
        {
            if (!Directory.Exists(@"logs"))
            {
                Directory.CreateDirectory(@"logs");
            }
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("logs" + $"/{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_log.txt",
                fileSizeLimitBytes: 3145728,
                rollOnFileSizeLimit: true,
#if DEBUG
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
#else
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
#endif
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz}[{Level:u3}]{Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private static void CheckDataExist()
        {
            string baseDir = Directory.GetCurrentDirectory();
            string imagesDir = Path.Combine(baseDir, "images");
            string imagesYTDir = Path.Combine(baseDir, "images/yt");
            string imagesTwitchDir = Path.Combine(baseDir, "images/twitch");
            string imagesRadioDir = Path.Combine(baseDir, "images/radio");
            string dataDir = Path.Combine(baseDir, "data");
            string tempDir = Path.Combine(baseDir, "temp");

            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
                Directory.CreateDirectory(imagesYTDir);
                Directory.CreateDirectory(imagesTwitchDir);
                Directory.CreateDirectory(imagesRadioDir);
                CopyFiles(Path.Combine(tempDir, "images"), imagesDir);
                CopyFiles(Path.Combine(tempDir, "images/yt"), imagesYTDir);
                CopyFiles(Path.Combine(tempDir, "images/twitch"), imagesTwitchDir);
                CopyFiles(Path.Combine(tempDir, "images/radio"), imagesRadioDir);
            }
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
                CopyFiles(Path.Combine(tempDir, "data"), dataDir);
            }
            if (Directory.Exists(tempDir))
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch(Exception ex)
                {
                    Log.Logger.Error("*CheckDataExist Delete Temp* msg: " + ex);
                }
            }
        }

        private static void CopyFiles(string tempDir, string destDir)
        {
            string[] files = Directory.GetFiles(tempDir);
            foreach (var i in files)
            {
                string fileName = Path.GetFileName(i);
                string destFile = Path.Combine(destDir, fileName);
                File.Copy(i, destFile, true);
            }
        }

        public static bool isValidImage(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath) || !imageExtensions.Contains(Path.GetExtension(imgPath).ToUpper()))
            {
                return false;
            }
            return true;
        }

    }
}
