using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triage.Models
{
    class Rescuer
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("rescuerID")]
        public string RescuerID { get; set; }
        [BsonElement("teamID")]
        public string TeamID { get; set; }
        [BsonElement("deviceName")]
        public string DeviceName { get; set; }
        [BsonElement("reports")]
        public List<TriageReport> Reports { get; set; } = new List<TriageReport>();

        public Rescuer(TriageReport report)
        {
            RescuerID = report.RescuerID;
            TeamID = "TEAM1";
            DeviceName = "hujawej";
        }
    }
}
