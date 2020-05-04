using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ProjetProgAvENSC1A.Models
{
    class Promotion : EntryType
    {
        public int GraduationYear { get; set;}
        public string Name { get; set;}

        [JsonIgnore]
        public FormYear Grade { get; set;}

        [JsonPropertyName("Grade")]
        public string JsonFormYUUID { get; set; }

        [JsonIgnore]
        public List<Student> Students { get; set;}

        [JsonPropertyName("Students")]
        public List<string> JsonStudUUID { get; set;}
    }
}
