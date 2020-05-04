﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetProgAvENSC1A.Models
{
    class Project : EntryType
    {
        public string Topic { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        // Les livrables sont dépendants du projet à l'inverse des 
        // autres attributs donc peuvent être stockés uniquement
        // dans l'objet projet
        public List<Deliverable> Deliverables { get; set; }


        [JsonIgnore]
        public TimeSpan Duration => EndDate - StartDate;
        //Form : NbDays.NbHours:NbMinutes:NbSeconds Write Duration.Days for collect number of days

        [JsonIgnore]
        public List<Promotion> Promotions { get; set; }

        [JsonIgnore]
        public List<Course> Courses { get; set; }

        [JsonIgnore]
        public List<Person> Contributors { get; set; }


        [JsonPropertyName("Promotions")] 
        public List<string> JsonPromUUID { get; set; }
        
        [JsonPropertyName("Courses")] 
        public List<string> JsonCoursUUID { get; set; }
        
        [JsonPropertyName("Contributors")] 
        public List<string> JsonPersUUID { get; set; }
    }
}