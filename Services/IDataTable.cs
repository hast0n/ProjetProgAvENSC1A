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
        /// <summary>
        /// Get the entries of the table.
        /// </summary>
        public List<EntryType> Entries { get; }

        /// <summary>
        /// Get an entry using its index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EntryType this [int index] => Entries[index];

        /// <summary>
        /// Get an entry using its UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public EntryType this [string uuid] => Entries.Find(e => e.UUID.Equals(uuid));

        /// <summary>
        /// Append an entry to the table.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool AddEntry(EntryType entry);

        /// <summary>
        /// Update an entry value in the table.
        /// </summary>
        /// <param name="oldEntry"></param>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        public bool UpdateEntry(EntryType oldEntry, EntryType newEntry);

        /// <summary>
        /// Remove an entry from the table.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool RemoveEntry(string uuid);

        /// <summary>
        /// Load data Json files from storage.
        /// </summary>
        /// <returns></returns>
        public Task<bool> LoadFromStorage();

        /// <summary>
        /// Write data to Json files in storage
        /// </summary>
        /// <returns></returns>
        public Task<bool> WriteToStorage();

        /// <summary>
        /// Wipe the content of the table
        /// </summary>
        /// <returns></returns>
        public bool DropContent();
    }
}
