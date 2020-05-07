using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class CourseDataTable : IDataTable
    {
        private const string filePath = Constants.COURSE_FILEPATH;

        private readonly List<EntryType> _entries;
        public List<EntryType> Entries => _entries.ToList(); // returns a copy of the entry list
        
        public CourseDataTable()
        {
            _entries = new List<EntryType>();
        }

        public bool AddEntry(EntryType entry)
        {
            try
            {
                _entries.Add(entry);
            }
            catch (Exception)
            {
                return false;
            }

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
            var tempEntries = new List<Course>();

            try
            {
                await using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<Course>>(fs);

                tempEntries = fileDump.Result;
            }
            catch (JsonException)
            {
                tempEntries = new List<Course>();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tempEntries.ForEach(entry =>
                {
                    entry.Teachers = entry.JsonTeachUUID.ConvertAll(
                        uuid => (Teacher)App.DB[DBTable.Person][uuid]);
                });

                _entries.AddRange(tempEntries);
            }

            return true;
        }

        public async Task<bool> WriteToStorage()
        {
            var tempEntries = Entries.ConvertAll(entry => (Course)entry);

            try
            {
                tempEntries.ForEach(entry =>
                {
                    Course c = (Course)entry;

                    c.JsonTeachUUID = c.Teachers.ConvertAll(teacher => teacher.UUID);
                });

                await using FileStream fs = File.Open(filePath, FileMode.Truncate);

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
