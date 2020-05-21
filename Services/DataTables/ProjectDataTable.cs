using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class ProjectDataTable : IDataTable
    {
        private string filePath = Constants.PROJECT_FILEPATH;
        
        private readonly List<EntryType> _entries;
        public List<EntryType> Entries => _entries.ToList(); // returns a copy of the entry list

        public ProjectDataTable()
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
            var tempEntries = new List<Project>();

            try
            {
                await using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<Project>>(fs);

                tempEntries = fileDump.Result;
            }
            catch (JsonException)
            {
                tempEntries = new List<Project>();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tempEntries.ForEach(entry =>
                {
                    entry.Courses = entry.JsonCoursUUID.ConvertAll(
                        uuid => (Course)App.DB[DBTable.Courses][uuid]);

                    entry.Promotions = entry.JsonPromUUID.ConvertAll(
                        uuid => (Promotion)App.DB[DBTable.Promotion][uuid]);

                    entry.Contributors = entry.JsonPersUUID.ToDictionary(
                        p => (Role)int.Parse(p.Key),
                        p => (Person)App.DB[DBTable.Person][p.Value]
                    );
                });

                _entries.AddRange(tempEntries);
            }

            return true;
        }

        public async Task<bool> WriteToStorage()
        {
            var tempEntries = Entries.ConvertAll(entry => (Project)entry);

            try
            {
                tempEntries.ForEach(entry =>
                {
                    entry.JsonCoursUUID = entry.Courses.ConvertAll(course => course.UUID);

                    entry.JsonPersUUID = entry.Contributors.ToDictionary(
                        p => $"{(int)p.Key}",
                        p => p.Value.UUID
                    );

                    entry.JsonPromUUID = entry.Promotions.ConvertAll(prom => prom.UUID);
                });

                await using FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write);

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