using Newtonsoft.Json;
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
        public string TeamId { get; set; }
        public string TimeStamp { get; set; }
        public SensorPacket SensorData { get; set; }
        public CarePriority Priority { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
        
    };

    
}
