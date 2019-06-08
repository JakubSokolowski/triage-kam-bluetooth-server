using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Triage.Bluetooth.Advertising;
using Triage.Models;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Triage.Bluetooth.Connection
{
    class FieldReportServer : BluetoothServer
    {
        private new static readonly Guid RfcommServiceUuid = Guid.Parse("00001101-0000-1000-8000-00805F9B34FB");
        private new const string SdpServiceName = "KAM BT Report Server";

        protected override void OnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Parallel.Invoke(() => ConnectionHandler(args));
        }
        protected override async void ConnectionHandler(StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                PrintClientInfo(args.Socket.Information);

                var socket = args.Socket;
                var writer = new DataWriter(socket.OutputStream);
                var reader = new DataReader(socket.InputStream);

                // Read incoming msg length
                uint readLength = await reader.LoadAsync(22);
                if (readLength < sizeof(uint)) return;

                string message = reader.ReadString(readLength);
                if (message == "METHOD")
                {
                    SendTriageMethod(socket);
                }
                else
                {   // Report
                    SaveTriageReport(socket, message);
                    Console.WriteLine(message);
                    Console.WriteLine("METHOD: START");
                }
           
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }  

        private void PrintClientInfo(StreamSocketInformation information)
        {
            Console.WriteLine("Handling new Connection...");
            Console.WriteLine(String.Format("RemoteAddress: {0}", information.RemoteHostName));
            Console.WriteLine(String.Format("RemoteHostName: {0}", information.RemoteAddress));
            Console.WriteLine(String.Format("RemoteServiceName: {0}", information.RemoteServiceName));
        }

        private async void SendTriageMethod(StreamSocket socket)
        {
            Console.WriteLine("Sending triage method...");
            string response = "START";
            var wrt = new DataWriter(socket.OutputStream);
            wrt.WriteString(response + Environment.NewLine);
            await wrt.StoreAsync();
        }

        private async void SaveTriageReport(StreamSocket socket, string message)
        {
            try
            {
                Console.WriteLine("Saving triage report...");
                var report = new TriageReport();
                DBWrapper.HandleNewTriageReport(report);
                var wrt = new DataWriter(socket.OutputStream);
                string response = "ACK";
                wrt.WriteString(response + Environment.NewLine);
                await wrt.StoreAsync();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
         
        }

    }
}
