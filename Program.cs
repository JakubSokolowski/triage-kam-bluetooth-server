using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Triage.Bluetooth.Advertising;
using Triage.Bluetooth.Connection;

namespace Triage
{
    class Program
    {
        static void Main(string[] args)
        {
            //DBWrapper.Connect();
            var serv = new FieldReportServer();
            try
            {
                var listener = new SensorPacketListener();
                serv.InitializeRfCommServer();
                 

                Parallel.Invoke(() => listener.Start());
                Console.ReadLine();
            }
            catch (System.TypeLoadException ex)
            {
                Console.WriteLine("Your OS does not have BLE capabilities.");
                Console.WriteLine("Please make sure that you are running this program on Windows 10");
                Debug.WriteLine(ex.Message);
            }
         
        }

    }
}
