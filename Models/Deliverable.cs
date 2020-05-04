using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
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

    class Deliverable : EntryType
    {
        public TypeDeliverable Type { get; set; }
        public string Information { get; set; }
    }
}
