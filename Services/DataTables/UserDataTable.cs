using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services
{
    class UserDataTable : IDataTable
    {
        public List<EntryType> Entries { get; private set; }

        public bool AddEntry(EntryType entry)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEntry(EntryType entry)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntry(string id)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromStorage()
        {
            throw new NotImplementedException();
        }

        public bool WriteToStorage()
        {
            throw new NotImplementedException();
        }
    }
}
