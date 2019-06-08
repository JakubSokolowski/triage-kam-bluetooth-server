using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Triage.Utils;

namespace Triage.Models
{
    public class SensorPacket
    {
        private readonly byte[] packetData;

        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("timestamp")]
        public long Timestamp { get; set; }
        [BsonElement("sensorID")]
        public int SensorID { get; set; }
        [BsonElement("pulse")]
        public int Pulse { get; set; }
        [BsonElement("breathPerMinute")]
        public int BreathPerMinute { get; set; }
        [BsonElement("saturation")]
        public int Saturation { get;set; }

        public SensorPacket(byte [] data)
        {
            packetData = data;
            Timestamp = DateTime.Now.ToUnixTimestamp();
        
            SensorID =  BitConverter.ToInt32(packetData, 0);
            Pulse =  BitConverter.ToInt32(packetData, 4);
            BreathPerMinute =  BitConverter.ToInt32(packetData, 8);
            Saturation =  BitConverter.ToInt32(packetData, 12);
        }

        public SensorPacket()
        {
            packetData = new byte[10];
            Timestamp = DateTime.Now.ToUnixTimestamp();
            SensorID = new Random().Next(1, 4);
            Pulse = new Random().Next(0, 200);
            BreathPerMinute = new Random().Next(0, 50);
            Saturation = new Random().Next(0, 10);
        }

        public void Print()
        {
            Console.WriteLine(String.Format("Sensor ID      : {0}", SensorID));
            Console.WriteLine(String.Format("Timestamp      : {0}", Timestamp));
            Console.WriteLine(String.Format("Pulse          : {0}", Pulse));
            Console.WriteLine(String.Format("BreathPerMinute: {0}: ", BreathPerMinute));
            Console.WriteLine(String.Format("BloodSaturation: {0}: ", Saturation));
        }
    }
}
