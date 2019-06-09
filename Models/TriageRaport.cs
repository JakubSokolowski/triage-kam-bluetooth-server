using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using Triage.Utils;

namespace Triage.Models
{
    public enum CarePriority
    {
        green,
        yellow,
        red,
        black
    }

    public class TriageReport
    {
        private static Random random = new Random();
        [BsonId]
        public ObjectId ID { get; set; } = ObjectId.GenerateNewId();
        [BsonElement("teamID")]
        public string TeamId { get; set; }
        [BsonElement("rescuerID")]
        public string RescuerID { get; set; }
        [BsonElement("timestamp")]
        public long Timestamp { get; set; }
        [BsonElement("sensorData")]
        public SensorPacket SensorData { get; set; }
        [BsonElement("priority")]
        public CarePriority Priority { get; set; }
        [BsonElement("latitude")]
        public double Latitude { get; set; }
        [BsonElement("longitude")]
        public double Longitude { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
        
        public TriageReport()
        {
            TeamId = "TEAM1";
            RescuerID = "R1";
            Timestamp = DateTime.Now.ToUnixTimestamp();
            SensorData = new SensorPacket();
            Array values = Enum.GetValues(typeof(CarePriority));
            Random random = new Random();
            Priority = (CarePriority)values.GetValue(random.Next(values.Length));
            // 51.109328,17.0575903
            // 51.109547,17.0595267
            Latitude = GetRandomNumber(51.1080, 51.1099);
            Longitude = GetRandomNumber(17.0560, 17.0599);
            // 51.109547,17.0595267
        }

        public double GetRandomNumber(double minimum, double maximum)
        { 
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    };

    
}
