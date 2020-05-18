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

        // Après avoir rajouter la surchage ToString() dans Person
        // tu peux simplifier cette boucle en :
        // Course Anglais = new Course() { ... };
        // String.Join(", ", Anglais.Teachers)
        // Ou tout simplement créer une nouvelle propriété :
        public string TeachersNames => String.Join(", ", Teachers);
        // La méthode String.Join() vient enchainer des éléments en une seule chaine
        // de caractères avec le séparateur spécifier qui vient s'insérer
        // entre chacune de ces chaines. Il n'y aura donc de séparateur ni au début,
        // ni à la fin
    }
}
