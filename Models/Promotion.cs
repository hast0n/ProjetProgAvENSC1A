using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    class Promotion : EntryType
    {
        public int GraduationYear { get; private set;}
        public FormYear Grade { get; private set;}
        public List<Student> Students { get; private set;}
        public string Name { get; private set;}
    }
}
