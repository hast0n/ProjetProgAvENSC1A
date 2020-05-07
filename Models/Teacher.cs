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
    }
}
