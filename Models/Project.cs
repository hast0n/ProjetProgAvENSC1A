using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetProgAvENSC1A.Models
{
    class Project : EntryType
    {
        public string Topic { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<Person> Contributors { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Deliverable[] Deliverables { get; private set; }
        public List<Promotion> Promotions { get; private set; }

        //public DateTime Duration => EndDate - StartDate;
        public TimeSpan Duration => EndDate - StartDate; //Form : NbDays.NbHours:NbMinutes:NbSeconds Write Duration.Days for collect number of days

    }
}