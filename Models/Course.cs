using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ProjetProgAvENSC1A.Services;
using System.Linq;

namespace ProjetProgAvENSC1A.Models
{
    class Course : EntryType
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<Teacher> Teachers { get; set; }

        [JsonPropertyName("Teachers")]
        public List<string> JsonTeachUUID { get; set; }

        public List<EntryType> Projects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;
            return p.Courses.Contains(this);
        }).ToList();
    }
}
