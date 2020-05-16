using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class Teacher : Person
    {
        // Get all courses dispensed by this teacher
        public List<EntryType> Courses => App.DB[DBTable.Courses].Entries.Where(entry =>
        {
            Course c = (Course) entry;
            return c.Teachers.Contains(this);
        }).ToList();

        public List<EntryType> CurrentProjects => App.DB[DBTable.Project].Entries.Where(entry =>
        {
            Project p = (Project)entry;
            DateTime today = DateTime.UtcNow;
            TimeSpan diff = p.EndDate - today;
            return p.Contributors.ContainsValue(this) && (diff.Days>0);
        }).ToList();
    }
}
