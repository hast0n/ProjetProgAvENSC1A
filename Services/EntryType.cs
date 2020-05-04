using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    public class EntryType
    {
        public string UUID { get; private set; }

        public EntryType(string uuid = null)
        {
            this.UUID = uuid ?? Guid.NewGuid().ToString();
        }
    }
}
