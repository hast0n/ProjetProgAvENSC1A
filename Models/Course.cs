using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    class Course : EntryType
    {
        public string Name { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
