using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Models;

// TODO: Add JSON serializable fields to each class extending IDatabase

namespace ProjetProgAvENSC1A.Services
{
    interface IDataTable
    {
        public List<EntryType> Entries { get; }

        public object this [int index] => Entries[index];
        public object this [string uuid] => Entries.Find(e => e.UUID.Equals(uuid));


        public bool AddEntry(EntryType entry);

        public bool UpdateEntry(EntryType oldEntry, EntryType newEntry);

        public bool RemoveEntry(string uuid);

        public bool LoadFromStorage();

        public bool WriteToStorage();
    }
}
