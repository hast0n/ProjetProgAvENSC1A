using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetProgAvENSC1A.Models
{
    class Project : EntryType
    {
        public string Topic { get; set; }
        public List<Course> Courses { get; set; }
        public List<Person> Contributors { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Deliverable[] Deliverables { get; set; }
        public List<Promotion> Promotions { get; set; }

        //public DateTime Duration => EndDate - StartDate;
        public TimeSpan Duration => EndDate - StartDate; //Form : NbDays.NbHours:NbMinutes:NbSeconds Write Duration.Days for collect number of days

    }
}