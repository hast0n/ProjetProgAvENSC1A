using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ProjetProgAvENSC1A.Models
{
    class Course : EntryType
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<Teacher> Teachers { get; set; }

        [JsonPropertyName("Teachers")]
        public List<string> JsonTeachUUID { get; set; }
    }
}
