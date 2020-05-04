using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class PersonDataTable : IDataTable
    {
        private const string filePath = Constants.PERSON_FILEPATH;

        public List<EntryType> Entries { get; }

        public bool AddEntry(EntryType entry)
        {
            try
            {
                Entries.Add(entry);
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool UpdateEntry(EntryType oldEntry, EntryType newEntry)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntry(string uuid)
        {
            try
            {
                Entries.RemoveAll(entry => entry.UUID.Equals(uuid));
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool LoadFromStorage()
        {
            var tempEntries = new List<Person>();

            try
            {
                using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<Person>>(fs);

                tempEntries = fileDump.Result;
            }
            catch (JsonException)
            {
                // Json file messed up; rewriting (and losing potential data)
                tempEntries = new List<Person>();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Entries.AddRange(tempEntries);
            }

            return true;
        }

        public bool WriteToStorage()
        {
            try
            {
                using FileStream fs = File.Open(filePath, FileMode.Truncate);

                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new PersonConverter());

                var personEntries = Entries.ConvertAll(entry => (Person)entry);

                JsonSerializer.SerializeAsync(fs, personEntries, options);
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
