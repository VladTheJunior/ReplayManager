using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ReplayManagerUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double progress;
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                NotifyPropertyChanged();
            }
        }

        private long downloadedSize;
        public long DownloadedSize
        {
            get { return downloadedSize; }
            set
            {
                downloadedSize = value;
                NotifyPropertyChanged();
            }
        }

        private long updatesSize;
        public long UpdatesSize
        {
            get { return updatesSize; }
            set
            {
                updatesSize = value;
                NotifyPropertyChanged();
            }
        }

        private string updateName;
        public string UpdateName
        {
            get { return updateName; }
            set
            {
                updateName = value;
                NotifyPropertyChanged();
            }
        }

        private string progressText;
        public string ProgressText
        {
            get { return progressText; }
            set
            {
                progressText = value;
                NotifyPropertyChanged();
            }
        }

        private string availableVersion;
        public string AvailableVersion
        {
            get { return availableVersion; }
            set
            {
                availableVersion = value;
                NotifyPropertyChanged();
            }
        }

        public class Update
        {
            public string name { get; set; }
            public string url { get; set; }
            public string md5 { get; set; }
            public long size { get; set; }
            public string install_path { get; set; }

        }

        public string CurrentVersion
        {
            get
            {
                return "0.0.6";
            }
        }
        public string AvailableVersionUrl
        {
            get
            {
                return "https://github.com/VladTheJunior/ReplayManager";
            }
        }

        public class Updates
        {
            public string version { get; set; }
            public string url { get; set; }
            public List<Update> files { get; set; } = new List<Update>();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = e.Uri.ToString(),
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        public Updates ServerUpdates { get; set; } = new Updates();
        public Updates ClientUpdates { get; set; } = new Updates();
        public Queue<Update> NewUpdates { get; set; } = new Queue<Update>();

        static async Task<string> CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                byte[] data;
                using (FileStream SourceStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    data = new byte[SourceStream.Length];
                    await SourceStream.ReadAsync(data, 0, (int)SourceStream.Length);
                }

                using var stream = new MemoryStream(data);
                var hash = await md5.ComputeHashAsync(stream);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            }
        }

        private void DownloadFile(Queue<Update> urls)
        {
            if (urls.Any())
            {
                previousDownloaded = 0;
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = urls.Dequeue();
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Environment.CurrentDirectory, url.install_path)));
                client.DownloadFileAsync(new Uri((url.url)), Path.Combine(Environment.CurrentDirectory, url.install_path));
                UpdateName = url.name;


                return;
            }

            using (var batFile = new StreamWriter(File.Create("Update.bat")))
            {
                string file = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
                batFile.WriteLine("@ECHO OFF");
                batFile.WriteLine("TIMEOUT /t 1 /nobreak > NUL");
                batFile.WriteLine("TASKKILL /F /IM \"{0}\" > NUL", file);
                batFile.WriteLine("IF EXIST \"{0}\" MOVE \"{0}\" \"{1}\"", file + ".upd", file);
                batFile.WriteLine("IF EXIST \"{0}\" MOVE \"{0}\" \"{1}\"", Path.GetFileNameWithoutExtension(file) + ".dll.upd", Path.GetFileNameWithoutExtension(file) + ".dll");
                batFile.WriteLine("DEL \"%~f0\" & START \"\" /B \"{0}\"", "ReplayManager.exe");
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("Update.bat");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            Process.Start(startInfo);
            Environment.Exit(0);
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            DownloadFile(NewUpdates);
        }
        private long previousDownloaded = 0;
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadedSize += e.BytesReceived - previousDownloaded;
            Progress = (double)DownloadedSize / (double)UpdatesSize;
            ProgressText = "Downloading: " + FormatFileSize(DownloadedSize) + " of " + FormatFileSize(UpdatesSize);
            previousDownloaded = e.BytesReceived;
        }

        public static string FormatFileSize(long bytes)
        {
            var unit = 1024;
            if (bytes < unit) { return $"{bytes} B"; }

            var exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return $"{bytes / Math.Pow(unit, exp):F2} {("KMGTPE")[exp - 1]}B";
        }

        async Task<string> HttpGetAsync(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
                Task<System.IO.Stream> result = hc.GetStreamAsync(URI);

                System.IO.Stream vs = await result;
                using (StreamReader am = new StreamReader(vs, Encoding.UTF8))
                {
                    return await am.ReadToEndAsync();
                }
            }
            catch
            {
                return "error";
            }
        }

        async Task<Updates> CheckUpdates()
        {
            string json = await HttpGetAsync("https://raw.githubusercontent.com/VladTheJunior/ReplayManagerUpdates/master/Updates.json");
            Updates res = new Updates();
            try
            {
                res = JsonSerializer.Deserialize<Updates>(json);
            }
            catch
            {
            }
            return res;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream);
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            AvailableVersion = "checking...";
            int index = 1;
            var updateFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories).Where(path => !path.Contains(".git") && Path.GetFileName(Path.GetDirectoryName(path)) != "Screenshots"
                    && Path.GetFileName(path) != "Updates.json"
                    && Path.GetFileName(path) != ".gitignore" && Path.GetFileName(Path.GetDirectoryName(path)) != "Output");
            foreach (string path in updateFiles)
            {
                Progress = (double)index / (double)updateFiles.Count();
                ClientUpdates.files.Add(new Update { name = Path.GetFileName(path), size = new FileInfo(path).Length, install_path = path.Remove(0, Environment.CurrentDirectory.Length + 1), md5 = await CalculateMD5(path), url = new Uri(new Uri("https://raw.githubusercontent.com/VladTheJunior/ReplayManagerUpdates/master/"), path.Remove(0, Environment.CurrentDirectory.Length + 1)).ToString() });
                ProgressText = $"Checking: file {index} of {updateFiles.Count()}";
                UpdateName = Path.GetFileName(path);
                index++;
            }


            ClientUpdates.version = CurrentVersion;
            ClientUpdates.url = AvailableVersionUrl;
            await File.WriteAllTextAsync("Updates.json", JsonSerializer.Serialize(ClientUpdates));


            ServerUpdates = await CheckUpdates();

            if (ServerUpdates.files.Count > 0)
            {
                var differences = ServerUpdates.files.Where(s => !ClientUpdates.files.Any(c => c.install_path == s.install_path && c.md5 == s.md5));
                foreach (Update f in differences)
                {
                    if (f.name == "ReplayManagerUpdater.exe" || f.name == "ReplayManagerUpdater.dll")
                        f.install_path += ".upd";
                    NewUpdates.Enqueue(f);
                }
            }
            if (NewUpdates.Count > 0)
            {
                AvailableVersion = ServerUpdates.version;
                foreach (var process in Process.GetProcessesByName("ReplayManager"))
                {
                    process.Kill();
                }
            }
            else
            {
                AvailableVersion = "up-to-dated";
            }
            Progress = 0;
            DownloadedSize = 0;
            UpdatesSize = NewUpdates.Sum(x => x.size);
            DownloadFile(NewUpdates);

        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
