using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class UserDataTable : IDataTable
    {
        private string filePath = Constants.USER_FILEPATH;

        private readonly List<EntryType> _entries;
        public List<EntryType> Entries => _entries.ToList(); // returns a copy of the entry list

        public UserDataTable()
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
            var tempEntries = new List<User>();

            try
            {
                await using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<User>>(fs);

                tempEntries = fileDump.Result;
            }
            catch (JsonException)
            {
                tempEntries = new List<User>();
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
            var tempEntries = Entries.ConvertAll(entry => (User)entry);

            try
            {
                await using FileStream fs = File.Open(filePath, FileMode.Truncate, FileAccess.Write);

                var options = new JsonSerializerOptions { WriteIndented = true };

                await JsonSerializer.SerializeAsync(fs, tempEntries, options);
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
    }
}
