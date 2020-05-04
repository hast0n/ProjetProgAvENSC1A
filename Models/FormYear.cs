using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class FormYear : EntryType
    {
        public Constants.GradeYear GradeName{ get; set; }

        [JsonIgnore]
        public List<Course> Courses { get; set;}

        [JsonPropertyName("Courses")]
        public List<string> JsonCoursUUID { get; set; }
    }
}
