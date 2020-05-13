using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>This property returns all projects associated to
        /// this formation year.
        /// </summary>
        public List<EntryType> AllProjects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;
            return p.Promotions.Any(prom => prom.Grade.Equals(this));
        }).ToList();

        /// <summary>This property returns projects associated to
        /// this formation year for the current promotion.
        /// </summary>
        public List<EntryType> CurrentProjects => App.DB[DBTable.Promotion].Entries.Where(entry =>
        {
            Promotion prom = (Promotion) entry;
            return prom.Grade.Equals(this) && prom.isCurrentPromotion;
        }).ToList();
    }
}
