using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triage.Models
{
    class ActionConfig
    {
        [BsonId]
        public ObjectId ID { get; set; } = ObjectId.GenerateNewId();
        public bool IsUsed { get; set; }
        public string Name { get; set; }
        public int VictimsNum { get; set; } = 0;
        public int RescuersNum { get; set; } = 0;
    }
}
