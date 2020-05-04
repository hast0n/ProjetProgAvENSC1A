﻿using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;


namespace ProjetProgAvENSC1A.Models
{
    class Person : EntryType
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Constants.Gender Sexe { get; set; }

        public void InvolvedInProject() 
        { 
            //work on the database
        }
    }
}
