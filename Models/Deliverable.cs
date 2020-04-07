using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    class Deliverable
    {
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

        public TypeDeliverable[] Livrable { get; private set; }
        public string DeliverableTypeInfo { get; private set; }

    }
}
