using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Triage.Bluetooth.Connection
{
    public abstract class BluetoothServer
    {
        public bool IsRunning { get; set; } = true;
        public bool ServerInitiated { get; private set; }

        protected static readonly Guid RfcommServiceUuid = Guid.Parse("00001101-0000-1000-8000-00805F9B34FB");
        protected const UInt16 SdpServiceNameAttributeId = 0x100;
        protected const byte SdpServiceNameAttributeType = (4 << 3) | 5;
        protected const string SdpServiceName = "BlueToothServer";

        protected RfcommServiceProvider rfcommProvider;
        protected StreamSocketListener socketListener;

        protected void Disconnect()
        {
            if (rfcommProvider != null)
            {
                rfcommProvider.StopAdvertising();
                rfcommProvider = null;
            }

            if (socketListener != null)
            {
                socketListener.Dispose();
                socketListener = null;
            }

            ServerInitiated = false;
        }
        public async void InitializeRfCommServer()
        {
            try
            {
                Console.WriteLine("Starting Serwer..");
                rfcommProvider = await RfcommServiceProvider.CreateAsync(RfcommServiceId.FromUuid(RfcommServiceUuid));

                // Create a listener for this service and start listening
                socketListener = new StreamSocketListener();
                socketListener.ConnectionReceived += OnConnectionReceived;

                await socketListener.BindServiceNameAsync(rfcommProvider.ServiceId.AsString(),
                   SocketProtectionLevel.PlainSocket);

                // Set the SDP attributes and start Bluetooth advertising
                InitializeServiceSdpAttributes(rfcommProvider);
                rfcommProvider.StartAdvertising(socketListener);
                ServerInitiated = true;
                Console.WriteLine("Server initialized..");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                ServerInitiated = false;
            }
        }
        protected void InitializeServiceSdpAttributes(RfcommServiceProvider rfcommProvider)
        {
            var sdpWriter = new DataWriter();

            // Write the Service Name Attribute.

            sdpWriter.WriteByte(SdpServiceNameAttributeType);

            // The length of the UTF-8 encoded Service Name SDP Attribute.
            sdpWriter.WriteByte((byte)SdpServiceName.Length);

            // The UTF-8 encoded Service Name value.
            sdpWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            sdpWriter.WriteString(SdpServiceName);

            // Set the SDP Attribute on the RFCOMM Service Provider.
            rfcommProvider.SdpRawAttributes.Add(SdpServiceNameAttributeId, sdpWriter.DetachBuffer());
        }
        protected abstract void OnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args);
        protected abstract void ConnectionHandler(StreamSocketListenerConnectionReceivedEventArgs args);
    }
}
