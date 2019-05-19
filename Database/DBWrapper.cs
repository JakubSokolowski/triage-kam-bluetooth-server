using MongoDB.Driver;
using System;
using Triage.Models;

namespace Triage.Bluetooth.Advertising
{
  
    public static class DBWrapper
    {
        private static readonly string connectionString = "mongodb+srv://namidad:Namidad12@triage-su1vl.gcp.mongodb.net/triage?retryWrites=true";
        private static MongoClient client;
        private static IMongoCollection<Victim> victims;

        public static void Connect()
        {
            try
            {
                Console.WriteLine("Connecting...");
                client = new MongoClient(connectionString);
                var db = client.GetDatabase("triage");
                victims = db.GetCollection < Victim > ("victims");
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static async void SaveSensorPacket(SensorPacket packetData)
        {
            var filter = Builders<Victim>.Filter.Eq(v => v.SensorId, packetData.SensorId);
            var update = Builders<Victim>.Update.Push(v => v.LifeLine, packetData);
            await victims.FindOneAndUpdateAsync(filter, update);
            Console.WriteLine(String.Format("Saved packet with for sensor {0}", packetData.SensorId));
        }

        public static async void SaveTriageReport(TriageReport triageReport)
        {
            var filter = Builders<Victim>.Filter.Eq(v => v.SensorId, triageReport.SensorData.SensorId);
            var update = Builders<Victim>.Update.Push(v => v.Reports, triageReport);
            await victims.FindOneAndUpdateAsync(filter, update);

        }

    }
}