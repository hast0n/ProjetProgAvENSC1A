using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Services
{
    public class Constants
    {
        public const string ANY = "any";
        public const string INPUT = "input";
        public const string COLOR = "color";
        public const string SELECTOR = "selector";

        public const string CARRIAGE_RETURN = "\r";
        public const string LINE_FEED = "\n";
        public const string BACKSPACE = "\b";

        public const string TYPE = "type";
        public const string INDEX = "index";
        public const string TEXT = "text";
        public const string REPLACEMENT = "replacement";
        public const string REGEX = "regex";
        public const string LENGTH = "length";
        public const string VALUE = "value";
        public const string SELECTED = "selected";
        
        
        public const string SELECTOR_BACKWARD = "z";
        public const string SELECTOR_FORWARD = "s";

        public const string PROJECT_FILEPATH = @"data\projects.json";
        public const string PERSON_FILEPATH = @"data\persons.json";
        public const string COURSE_FILEPATH = @"data\courses.json";
        public const string FORMYEAR_FILEPATH = @"data\formyears.json";
        public const string PROMOTION_FILEPATH = @"data\promotions.json";
        

        public enum TypeDeliverable
        {
            SiteWeb,
            Rapport,
            MaquetteFonctionnelle,
            Documentation,
            CahierDesCharges,
            ApplicationLogicielle,
            Autre
        }

        public enum Gender
        {
            Feminin,
            Masculin, 
            Autre
        }

        public enum GradeYear
        {
            Y1,
            Y2,
            Y3
        }
    }
}
