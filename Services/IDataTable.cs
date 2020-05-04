using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

// TODO: Add JSON serializable fields to each class extending IDatabase

namespace ProjetProgAvENSC1A.Services
{
    interface IDataTable
    {
        public List<EntryType> Entries { get; }

        public EntryType this [int index] => Entries[index];
        public EntryType this [string uuid] => Entries.Find(e => e.UUID.Equals(uuid));

        public bool AddEntry(EntryType entry);

        public bool UpdateEntry(EntryType oldEntry, EntryType newEntry);

        public bool RemoveEntry(string uuid);

        // use of asynchronous json serializing process
        // -> switch to async method
        public Task<bool> LoadFromStorage();

        public Task<bool> WriteToStorage();

        public bool DropContent();
    }
}
