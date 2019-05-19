using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Triage.Models
{
    class Victim
    {
        public ObjectId Id { get; set; }
        public string color { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string injury { get; set; }
        public int __v { get; set; }
        public int SensorId { get; set; }
        public List<SensorPacket> LifeLine { get; set; } = new List<SensorPacket>();
        public List<TriageReport> Reports { get; set; }

        public void Print()
        {
            Console.WriteLine(Id);
            Console.WriteLine(lat);
            Console.WriteLine(lng);
            Console.WriteLine();
        }
    }
}
