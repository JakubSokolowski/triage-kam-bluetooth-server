using System;

namespace Triage.Models
{
    public class SensorPacket
    {
        private readonly byte[] packetData;
        public DateTime TimeStamp { get; set; }
        public int SensorId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int HeartBeat { get; set; }
        public int Breathing { get; set; }

        public SensorPacket(byte [] data)
        {
            packetData = data;
            TimeStamp = DateTime.Now;
            SensorId =  BitConverter.ToChar(packetData, 0) - '0';
            Longitude = BitConverter.ToSingle(packetData, 2);
            Latitude = BitConverter.ToSingle(packetData, 6);
            HeartBeat = BitConverter.ToInt32(packetData, 10) - '0';
            Breathing = BitConverter.ToInt32(packetData, 11) - '0';
        }
    }
}
