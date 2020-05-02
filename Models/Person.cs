using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;


namespace ProjetProgAvENSC1A.Models
{
    class Person : EntryType
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public Constants.Gender Sexe { get; private set; }

        public void InvolvedInProject() 
        { 
            //work on the database
        }
    }
}
