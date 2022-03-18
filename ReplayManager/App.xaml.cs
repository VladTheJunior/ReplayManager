using ReplayManager.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ReplayManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            SingleInstanceApp pipe = new SingleInstanceApp();

            if (await pipe.WriteLine("ReplayManager", "start"))
            {
                
                if (e.Args != null)
                {
                    for (int i = 0; i < e.Args.Length; i++)
                    {
                        await pipe.WriteLine("ReplayManager", e.Args[i]);
                        
                    }
                }
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);

        }
    }
}
