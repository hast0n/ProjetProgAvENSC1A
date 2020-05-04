using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTables
{
    class ProjectDataTable : IDataTable
    {
        private string filePath = @"data\projects.json";

        public List<EntryType> Entries { get; }
        public List<Project> TempEntries { get; private set; }

        public ProjectDataTable()
        {
            Entries = new List<EntryType>();
            TempEntries = new List<Project>();
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
            try
            {
                Directory.CreateDirectory("data");

                using FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
                var fileDump = JsonSerializer.DeserializeAsync<List<Project>>(fs);

                TempEntries = fileDump.Result;
            }
            catch (JsonException)
            {
                TempEntries = new List<Project>();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                TempEntries.ForEach(entry =>
                {
                    entry.Courses = entry.JsonCoursUUID.ConvertAll(
                        uuid => (Course) App.DB[DBTable.Courses][uuid]);

                    entry.Promotions = entry.JsonPromUUID.ConvertAll(
                        uuid => (Promotion) App.DB[DBTable.Promotion][uuid]);

                    entry.Contributors = entry.JsonPersUUID.ConvertAll(
                        uuid => (Person) App.DB[DBTable.Person][uuid]);
                });

                Entries.AddRange(TempEntries);
            }

            return true;
        }

        public bool WriteToStorage()
        {
            try
            {
                Directory.CreateDirectory("data");

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