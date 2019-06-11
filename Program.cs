using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Triage.Bluetooth.Advertising;
using Triage.Bluetooth.Connection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;

namespace Triage
{
    class Program
    {
        static void Main(string[] args)
        {
            ConventionPack pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention(),
            };

            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
            DBWrapper.Connect();
            var serv = new TriageReportServer();
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
