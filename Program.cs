using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triage.Bluetooth.Advertising;

using Windows.Devices.Bluetooth.Advertisement;
using Triage.Bluetooth.Connection;

namespace Triage
{
    class Program
    {
        static void Main(string[] args)
        {
            DBWrapper.Connect();
            var serv = new FieldReportServer();
            var listener = new SensorPacketListener();
            serv.InitializeRfCommServer();
            Parallel.Invoke(() => listener.Start());
            Console.ReadLine();
        }

    }
}
