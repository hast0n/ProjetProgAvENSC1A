using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class PersonDataTable : IDataTable
    {
        private const string FilePath = Constants.PERSON_FILEPATH;

        private readonly List<EntryType> _entries;
        public List<EntryType> Entries => _entries.ToList(); // returns a copy of the entry list

        public PersonDataTable()
        {
            _entries = new List<EntryType>();
        }

        public bool AddEntry(EntryType entry)
        {
            try
            {
                _entries.Add(entry);
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
                _entries.RemoveAll(entry => entry.UUID.Equals(uuid));
            }
            catch (Exception) { return false; }

            return true;
        }

        public async Task<bool> LoadFromStorage()
        {
            var tempEntries = new List<Person>();

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new PersonConverter());

                await using FileStream fs = File.Open(FilePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<Person>>(fs, options);

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
                _entries.AddRange(tempEntries);
            }

            return true;
        }

        public async Task<bool> WriteToStorage()
        {
            try
            {
                await using FileStream fs = File.Open(FilePath, FileMode.Create, FileAccess.Write);

                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new PersonConverter());

                var personEntries = Entries.ConvertAll(entry => (Person)entry);

                await JsonSerializer.SerializeAsync(fs, personEntries, options);
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DropContent()
        {
            try
            {
                _entries.Clear();
                WriteToStorage();
            }
            catch (Exception) { return false; }

            return true;
        }

        ///<summary>  This property returns Person of the chosen subclass 
        ///</summary>
        public List<Person> GetPersonsOfType<T>() => _entries
            .Where(entry => entry is T).ToList().ConvertAll(entry => (Person)entry); 
    }
}
