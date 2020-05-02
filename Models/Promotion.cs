using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    class Promotion : EntryType
    {
        public int GraduationYear { get; set;}
        public FormYear Grade { get; set;}
        public List<Student> Students { get; set;}
        public string Name { get; set;}
    }
}
