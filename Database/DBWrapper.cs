using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Triage.Models;

namespace Triage.Bluetooth.Advertising
{
  
    public static class DBWrapper
    {
        private static readonly string connectionString = "mongodb://localhost:27017";
        private static MongoClient client;
        private static IMongoCollection<Victim> victims;
        private static IMongoCollection<TriageReport> reports;
        private static IMongoCollection<Rescuer> rescuers;

        public static void Connect()
        {
            try
            {
                Console.WriteLine("Connecting...");
                client = new MongoClient(connectionString);
                var db = client.GetDatabase("triage");
                db.DropCollection("victims");
                db.CreateCollection("victims");
                victims = db.GetCollection < Victim > ("victims");                
                reports = db.GetCollection<TriageReport>("reports");
                rescuers = db.GetCollection<Rescuer>("rescuers");
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static async void InsertSensorPacket(SensorPacket packetData)
        {
            long count = victims.Find(v => v.VictimID == packetData.SensorID).CountDocuments();
            if (count > 0)
            {
                var filter = Builders<Victim>.Filter.Eq(v => v.VictimID, packetData.SensorID);
                var update = Builders<Victim>.Update.Push(v => v.SensorReads, packetData);
                await victims.FindOneAndUpdateAsync(filter, update);
                Console.WriteLine(String.Format("Saved packet with for sensor {0}", packetData.SensorID));
                packetData.Print();
            } 
        }

        public static void HandleNewTriageReport(TriageReport triageReport)
        {
            long count = victims.Find(v => v.VictimID == triageReport.SensorData.SensorID).CountDocuments();
            long rescuerCount = rescuers.Find(v => v.RescuerID == triageReport.RescuerID).CountDocuments();
            if(count > 0)
            {
                // Victim exists
               
                AddNewReportToVictim(triageReport);
            } else
            {
                InsertNewVictim(triageReport);
            }      

            if(rescuerCount > 0)
            {
                AddNewReportToRescuer(triageReport);
            }
            else
            {
                InsertNewRescuer(triageReport);
            }

        }

        private static async void AddNewReportToVictim(TriageReport report)
        {
            var filter = Builders<Victim>.Filter.Eq(v => v.VictimID, report.SensorData.SensorID);
            var update = Builders<Victim>.Update.Push(v => v.Reports, report).Set(v => v.CurrentPriority, report.Priority);
            await victims.FindOneAndUpdateAsync(filter, update);
        }

        private static async void AddNewReportToRescuer(TriageReport report)
        {
            var filter = Builders<Rescuer>.Filter.Eq(r => r.RescuerID, report.RescuerID);
            var update = Builders<Rescuer>.Update.Push(v => v.Reports, report);
            await rescuers.FindOneAndUpdateAsync(filter, update);
        }

        private static async void InsertNewVictim(TriageReport report)
        {
            var vic = new Victim(report);
            await  victims.InsertOneAsync(vic);
        }

        private static async void InsertNewRescuer(TriageReport report)
        {
            var res = new Rescuer(report);
            await rescuers.InsertOneAsync(res);
        }

    }
}