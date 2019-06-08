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
        public int BreathPerMinute { get; set; }
        public int Pulse { get ;set; }
        public int BloodSaturation { get;set; }

        public SensorPacket(byte [] data)
        {
            packetData = data;
            TimeStamp = DateTime.Now;
            SensorId =  BitConverter.ToInt32(packetData, 0);
            Pulse =  BitConverter.ToInt32(packetData, 4);
            BreathPerMinute =  BitConverter.ToInt32(packetData, 8);
            BloodSaturation =  BitConverter.ToInt32(packetData, 12);
            Latitude = BitConverter.ToInt32(packetData, 16);
            Longitude = BitConverter.ToInt32(packetData, 20);
        }

        public void Print()
        {
            Console.WriteLine(String.Format("Sensor ID      : {0}", SensorId));
            Console.WriteLine(String.Format("Timestamp      : {0}", TimeStamp));
            Console.WriteLine(String.Format("Pulse          : {0}", Pulse));
            Console.WriteLine(String.Format("BreathPerMinute: {0}: ", BreathPerMinute));
            Console.WriteLine(String.Format("BloodSaturation: {0}: ", BloodSaturation));
        }
    }
}
