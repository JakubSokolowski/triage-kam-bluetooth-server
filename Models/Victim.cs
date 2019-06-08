using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Triage.Models
{
    public enum VictimState
    {
        TRIAGED,
        NOT_TRIAGED
    }

    public class Victim
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("victimID")]
        public int VictimID {get; set;}
        [BsonElement("currentPriority")]
        public CarePriority CurrentPriority { get; set; }
        [BsonElement("state")]
        public VictimState State { get; set; } = VictimState.TRIAGED;
        [BsonElement("sensorReads")]
        public List<SensorPacket> SensorReads { get; set; } = new List<SensorPacket>();
        [BsonElement("reports")]
        public List<TriageReport> Reports { get; set; } = new List<TriageReport>();

        public void Print()
        {
            Console.WriteLine(ID);
            Console.WriteLine(VictimID);
        }

        public Victim()
        {

        }

        public Victim(TriageReport report)
        {
            VictimID = report.SensorData.SensorID;
            State = VictimState.TRIAGED;
            Reports.Add(report);
            SensorReads.Add(report.SensorData);
            CurrentPriority = report.Priority;
        }
    }
}
