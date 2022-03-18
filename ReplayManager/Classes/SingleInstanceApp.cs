using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ReplayManager.Classes
{
    public class SingleInstanceApp
    {
        private Thread thread;
        public event Action<string> ReceiveLine;
        public async Task StartServer(string pipeName)
        {
            thread = new Thread(async () =>
            {
                while (true)
                {
                    using var server = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    await server.WaitForConnectionAsync();

                    var reader = new StreamReader(server);
                    string line = await reader.ReadLineAsync();

                    if (line != null)
                        OnReceiveString(line);

                }
            });
            thread.Start();
        }

        protected virtual void OnReceiveString(string text) => ReceiveLine?.Invoke(text);

        public async Task<bool> WriteLine(string pipeName, string text, int connectTimeout = 100)
        {
            using var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.Asynchronous);

            try
            {
                await client.ConnectAsync(connectTimeout);
            }
            catch
            {
                return false;
            }
            if (!client.IsConnected)
                return false;

            var writer = new StreamWriter(client)
            {
                AutoFlush = true
            };
            await writer.WriteLineAsync(text);


            return true;

        }
    }
}
