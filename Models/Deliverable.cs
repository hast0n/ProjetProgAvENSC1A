using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    public enum DeliverableType
    {
        Website,
        Report,
        FunctionalModel,
        Documentation,
        Specifications,
        SoftwareApplication,
        Other
    }

    class Deliverable : EntryType
    {
        public DeliverableType Type { get; set; }
        public string Information { get; set; }
    }
}
