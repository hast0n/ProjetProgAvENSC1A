using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class Student : Person 
    {
        public string Student_ID { get; set; }
        
        public void PartOfPromotion()
        {
            //work on the database
        }
    }
}
