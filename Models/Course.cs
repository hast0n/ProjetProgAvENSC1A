using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    class Course : EntryType
    {
        public string Name { get; private set; }
        public List<Teacher> Teachers { get; private set; }
    }
}
