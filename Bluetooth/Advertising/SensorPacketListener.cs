using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triage.Models;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Triage.Utils;

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
            try
            {
                Console.WriteLine("NEW PACKET: ");
                var input = string.Format("0{0:X}", eventArgs.BluetoothAddress);
                // System.Console.WriteLine(input);
                var output = string.Join(":", Enumerable.Range(0, 6).Reverse()
                    .Select(i => input.Substring(i * 2, 2)));
                var test = string.Join(":", output.Split(':'));
                Console.WriteLine("  BT_ADDR : {0}", output); //return string.Format("0x{0:X}", temp);
                Console.WriteLine("  SIGNAL  : {0}", eventArgs.RawSignalStrengthInDBm);

                List<byte> packetData = new List<byte>();
                int count = 0;
                foreach (var section in eventArgs.Advertisement.DataSections)
                {
                    var dataReader = DataReader.FromBuffer(section.Data);
                    byte[] buffer = new byte[section.Data.Length];
                    dataReader.ReadBytes(buffer);
                    packetData.AddRange(buffer);
                    string hex = BitConverter.ToString(buffer);
                    string packet = hex.Replace("-", ":");
                    Console.WriteLine("  DATA {0}  : {1}", count, packet);
                    count++;
                }
                Console.WriteLine("  STR     : {0}", Encoding.Default.GetString(packetData.ToArray()));
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Inserting packets");
                for (int i = 1; i < 100; i++)
                {
                    SensorPacket pc = new SensorPacket();
                    pc.SensorID = i;
                    DBWrapper.InsertSensorPacket(pc);
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Error reeiving packet");
            }          
           

        }

        private void InitListener()
        {
            watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            watcher.SignalStrengthFilter.InRangeThresholdInDBm = -90;
            watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -95;
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            watcher.Received += OnAdvertisementReceived;
        }
        private string Reverse(string text)
        {
            if (text == null) return null;

            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

    }
}
