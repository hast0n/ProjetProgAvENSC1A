using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using ProjetProgAvENSC1A.Services;



namespace ProjetProgAvENSC1A.Models
{
    class Promotion : EntryType
    {
        public int GraduationYear { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public FormYear Grade { get; set; }

        [JsonPropertyName("Grade")]
        public string JsonFormYUUID { get; set; }

        [JsonIgnore]
        public List<Student> Students { get; set; }

        [JsonPropertyName("Students")]
        public List<string> JsonStudUUID { get; set; }

        public List<EntryType> Projects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;
            return p.Promotions.Contains(this);
        }).ToList();

        public bool CurrentPromotion
        {
            get
            {
                DateTime today = DateTime.UtcNow;
                if ((Grade.GradeName.Equals(Constants.GradeYear.Y1))&((this.GraduationYear == (today.Year + 2) & (today.Month <= 8)) ^ (this.GraduationYear == (today.Year + 3) & today.Month > 8))) { return true; }
                else if ((Grade.GradeName.Equals(Constants.GradeYear.Y2)) & ((this.GraduationYear == (today.Year + 1) & (today.Month <= 8)) ^ (this.GraduationYear == (today.Year + 2) & today.Month > 8))) { return true; }
                else if ((Grade.GradeName.Equals(Constants.GradeYear.Y3)) & ((this.GraduationYear == (today.Year) & (today.Month <= 8)) ^ (this.GraduationYear == (today.Year + 1) & today.Month > 8))) { return true; }
                else { return false; }
            }
        }
    }
}