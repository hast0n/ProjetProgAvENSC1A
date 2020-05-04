using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class ProjectDataTable : IDataTable
    {
        private string filePath = Constants.PROJECT_FILEPATH;

        public List<EntryType> Entries { get; }

        public ProjectDataTable()
        {
            Entries = new List<EntryType>();
        }

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
            var tempEntries = new List<Project>();

            try
            {
                using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
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
                        uuid => (Course) App.DB[DBTable.Courses][uuid]);

                    entry.Promotions = entry.JsonPromUUID.ConvertAll(
                        uuid => (Promotion) App.DB[DBTable.Promotion][uuid]);

                    entry.Contributors = entry.JsonPersUUID.ConvertAll(
                        uuid => (Person) App.DB[DBTable.Person][uuid]);
                });

                Entries.AddRange(tempEntries);
            }

            return true;
        }

        public bool WriteToStorage()
        {
            try
            {
                Entries.ForEach(entry =>
                {
                    Project p = (Project) entry;

                    p.JsonCoursUUID = p.Courses.ConvertAll(course => course.UUID);
                    p.JsonPersUUID = p.Contributors.ConvertAll(pers => pers.UUID);
                    p.JsonPromUUID = p.Promotions.ConvertAll(prom => prom.UUID);
                });

                using FileStream fs = File.Open(filePath, FileMode.Truncate);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var projectEntries = Entries.ConvertAll(entry => (Project) entry);

                JsonSerializer.SerializeAsync(fs, projectEntries, options);
            }
            catch (Exception) { return false; }
            
            return true;
        }
    }
}