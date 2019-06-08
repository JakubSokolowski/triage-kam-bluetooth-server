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
            Console.WriteLine("NEW PACKET: ");
           // var input = string.Format("0{0:X}", eventArgs.BluetoothAddress);
           //// System.Console.WriteLine(input);
           // var output = string.Join(":", Enumerable.Range(0, 6).Reverse()
           //     .Select(i => input.Substring(i * 2, 2)));
           // var test = string.Join(":", output.Split(':'));
           // Console.WriteLine(String.Format("  BT_ADDR OUTPUT: {0}", output)); //return string.Format("0x{0:X}", temp);
           // Console.WriteLine(String.Format("  SIGNAL:       : {0}", eventArgs.RawSignalStrengthInDBm));
          
            //List<byte> packetData = new List<byte>();
            //foreach (var section in eventArgs.Advertisement.DataSections)
            //{
            //    var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(section.Data);
            //    byte[] buffer = new byte[section.Data.Length];
            //    dataReader.ReadBytes(buffer);
            //    packetData.AddRange(buffer);
            //    string hex = BitConverter.ToString(buffer);
            //    string packet = hex;
            //    if(packet.StartsWith("06-00-01-09-20"))
            //        return;
            //    Console.WriteLine(String.Format("  DATA: {0}", packet));
            //}
            //Console.WriteLine("");
            //Console.WriteLine("");
            for(int i = 1 ; i < 4; i++)
            {
                SensorPacket pc = new SensorPacket();
                pc.SensorID = i;
                DBWrapper.InsertSensorPacket(pc);
            }
          
            // packet.Print();
          
            //if (packetData.Count >= 10)
            //{
            //    SensorPacket packet = new SensorPacket();
            //    // packet.Print();
            //    DBWrapper.InsertSensorPacket(packet);
            //}
            //var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
            //Console.WriteLine(String.Format("Advertisement:"));

        }

        private void InitListener()
        {
            watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            // Only activate the watcher when we're recieving values >= -80
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
