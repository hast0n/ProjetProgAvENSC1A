using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class UserDataTable : IDataTable
    {
        public List<EntryType> Entries { get; private set; }

        public bool AddEntry(EntryType entry)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEntry(EntryType oldEntry, EntryType newEntry)
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

        public Task<bool> LoadFromStorage()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WriteToStorage()
        {
            throw new NotImplementedException();
        }

        public bool DropContent()
        {
            throw new NotImplementedException();
        }
    }
}
