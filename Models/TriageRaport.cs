using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using Triage.Utils;

namespace Triage.Models
{
    public enum CarePriority
    {
        GREEN,
        YELLOW,
        RED,
        BLACK
    }

    public class TriageReport
    {
        [BsonId]
        public ObjectId ID { get; set; }
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
        }
    };

    
}
