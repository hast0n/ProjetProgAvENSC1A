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

        public string TeachersToString()
        {
            string teacher = "";
            for (int  i = 0; i < this.Teachers.Count; i++) 
            {
                if (i != 0 & i != (this.Teachers.Count - 1)) { teacher += ", "; }
                teacher += this.Teachers[i].FirstName + " " + this.Teachers[i].LastName;
                if (i%2==0 & i!=(this.Teachers.Count-1)) { teacher += "\n"; }
            }
            return teacher;
        }
    }
}
