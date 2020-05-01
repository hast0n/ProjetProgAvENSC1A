using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    class Deliverable : EntryType
    {
        public Constants.TypeDeliverable Type { get; private set; }
        public string Information { get; private set; }
    }
}
