using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Models
{
    public class EntryType
    {
        private Guid _uuid = Guid.NewGuid();

        public string UUID
        {
            get => _uuid.ToString();
            set => _uuid = Guid.Parse(value);
        }
    }
}
