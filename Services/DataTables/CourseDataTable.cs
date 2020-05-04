using System;
using System.Collections.Generic;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTable
{
    class CourseDataTable : IDataTable
    {
        private string filePath = @"data\courses.json";
        public List<EntryType> Entries { get; }

        public CourseDataTable()
        {
            Entries = new List<EntryType>();
        }

        public bool AddEntry(EntryType entry)
        {
            try
            {
                Entries.Add(entry);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
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
