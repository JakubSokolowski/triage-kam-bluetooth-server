using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triage.Models
{
    class User
    {
        [BsonId]
        public ObjectId ID { get; set; } = ObjectId.GenerateNewId();
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
