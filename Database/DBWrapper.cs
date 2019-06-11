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
        private static IMongoCollection<User> users;
        private static IMongoCollection<ActionConfig> actionConfigs;

        public static void Connect()
        {
            try
            {
                client = new MongoClient(connectionString);
                DropExistingCollections();
                CreateAndFillNewCollections();
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
            if(ShouldAddNewVictim(triageReport))
                InsertNewVictim(triageReport);
            else
                AddNewReportToVictim(triageReport);

            if(ShouldAddNewRescuer(triageReport))
                InsertNewRescuer(triageReport);
            else
                AddNewReportToRescuer(triageReport);
        }
        
        private static void DropExistingCollections()
        {
            var db = client.GetDatabase("triage");
            db.DropCollection("victims");
            db.DropCollection("rescuers");
            db.DropCollection("users");
            db.DropCollection("actionConfigs");
            Console.WriteLine("Dropped existing victims and users collection...");
        }

        private static void CreateAndFillNewCollections()
        {
            var db = client.GetDatabase("triage");
            victims = db.GetCollection<Victim>("victims");
            reports = db.GetCollection<TriageReport>("reports");
            rescuers = db.GetCollection<Rescuer>("rescuers");
            users = db.GetCollection<User>("users");
            actionConfigs = db.GetCollection<ActionConfig>("actionconfigs");

            FillUserCollection();
            FillActionConfigs();
        }

        private static void FillUserCollection()
        {
            List<User> newUsers = new List<User>
            {
                new User{Username = "admin", Password = "123123123"},
                new User{Username = "namidad", Password = "namidad12"}
            };
            users.InsertMany(newUsers);
        }

        private static void FillActionConfigs()
        {
            List<ActionConfig> configs = new List<ActionConfig>
            {
                new ActionConfig{Name = "START", IsUsed = true, RescuersNum = 0, VictimsNum = 0 },
                new ActionConfig{Name = "JUMPSTART", IsUsed = false, RescuersNum = 0, VictimsNum = 0 }
            };
            actionConfigs.InsertMany(configs);
        }
  
        private static bool ShouldAddNewRescuer(TriageReport triageReport)
        {
            long count = rescuers.Find(v => v.RescuerID == triageReport.RescuerID).CountDocuments();
            return count == 0;
        }

        private static bool ShouldAddNewVictim(TriageReport triageReport)
        {
            long count = victims.Find(v => v.VictimID == triageReport.SensorData.SensorID).CountDocuments();
            return count == 0;
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