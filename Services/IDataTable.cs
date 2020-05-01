using System;
using System.Collections.Generic;
using System.Text;

// TODO: Add JSON serializable fields to each class extending IDatabase

namespace ProjetProgAvENSC1A.Services
{
    interface IDataTable
    {
        public List<object> Entries { get; }

        public object this [int index] => Entries[index];


        public bool AddEntry(object entry);

        public bool UpdateEntry(object entry);

        public bool RemoveEntry(string id);

        public List<object> LoadFromStorage();

        public bool WriteToStorage();

    }
}
