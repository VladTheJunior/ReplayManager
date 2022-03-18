using Newtonsoft.Json;
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

        private string downloaded;
        public string Downloaded
        {
            get { return downloaded; }
            set
            {
                downloaded = value;
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

        public class Update
        {
            public string name { get; set; }
            public string url { get; set; }
            public string md5 { get; set; }
            public string install_path { get; set; }
        }

        public class Updates
        {
            public List<Update> files { get; set; } = new List<Update>();
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
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = urls.Dequeue();
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Environment.CurrentDirectory, url.install_path)));
                client.DownloadFileAsync(new Uri((url.url)), Path.Combine(Environment.CurrentDirectory, url.install_path));
                UpdateName = "Downloading: " + url.name;
                return;
            }

            ProgressText = "Download Complete";
            UpdateName = "";
            Downloaded = "";
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

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = (double)e.BytesReceived / (double)e.TotalBytesToReceive;
            Downloaded = "Downloaded: " + (e.BytesReceived / 1024).ToString("N0") + " KB from " + (e.TotalBytesToReceive / 1024).ToString("N0") + " KB";
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
                res = JsonConvert.DeserializeObject<Updates>(json);
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
            ProgressText = "Checking for updates...";

            foreach (string path in Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories))
            {
                if (!path.Contains(".git") && Path.GetFileName(Path.GetDirectoryName(path)) != "Screenshots"
                    && Path.GetFileName(path) != "Updates.json"
                    && Path.GetFileName(path) != ".gitignore" && Path.GetFileName(Path.GetDirectoryName(path)) != "Output")
                    ClientUpdates.files.Add(new Update { name = Path.GetFileName(path), install_path = path.Remove(0, Environment.CurrentDirectory.Length + 1), md5 = await CalculateMD5(path), url = new Uri(new Uri("https://raw.githubusercontent.com/VladTheJunior/ReplayManagerUpdates/master/"), path.Remove(0, Environment.CurrentDirectory.Length + 1)).ToString() });
            }
            await File.WriteAllTextAsync("Updates.json", JsonConvert.SerializeObject(ClientUpdates));


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

            if (!NewUpdates.Any())
                ProgressText = "No new updates.";
            else
            {
                ProgressText = "Getting updates...";
            }
            DownloadFile(NewUpdates);
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
