using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triage.Models;
using Windows.Devices.Bluetooth.Advertisement;

namespace Triage.Bluetooth.Advertising
{
    class SensorPacketListener
    {
        private BluetoothLEAdvertisementWatcher watcher;

        public SensorPacketListener()
        {
            InitListener();
        }
        public void Start()
        {
            watcher.Start();
            Console.WriteLine("Started packet listener...");
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            Console.WriteLine(String.Format("Advertisement:"));
            Console.WriteLine(String.Format("  BT_ADDR: {0}", eventArgs.BluetoothAddress));
            Console.WriteLine(String.Format("  FR_NAME: {0}", eventArgs.Advertisement.LocalName));
            List<byte> packetData = new List<byte>();
            foreach (var section in eventArgs.Advertisement.DataSections)
            {
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(section.Data);
                byte[] buffer = new byte[section.Data.Length];
                dataReader.ReadBytes(buffer);
                packetData.AddRange(buffer);
                string hex = BitConverter.ToString(buffer);
                Console.WriteLine(String.Format("  DATA: {0}", hex.Replace("-", " ")));
            }
            if(packetData.Count >= 24)
            {
                SensorPacket packet = new SensorPacket(packetData.ToArray());
                DBWrapper.SaveSensorPacket(packet);
            }
        }

        private void InitListener()
        {
            watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            // Only activate the watcher when we're recieving values >= -80
            watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;
            watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            watcher.Received += OnAdvertisementReceived;
        }
    }
}
