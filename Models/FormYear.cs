using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class FormYear : EntryType
    {
        public Constants.GradeYear GradeName{ get; set; }
        public List<Course> Courses { get; set;}
    }
}
