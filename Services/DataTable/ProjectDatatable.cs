using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Services.DataTable
{
    class ProjectDatatable : IDataTable
    {
        private string filePath = @"data\projects.json";
        public List<object> Entries { get; private set; }

        public bool AddEntry(object entry)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEntry(object entry)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntry(string id)
        {
            throw new NotImplementedException();
        }

        public List<object> LoadFromStorage()
        {
            throw new NotImplementedException();
        }

        public bool WriteToStorage()
        {
            throw new NotImplementedException();
        }

        //public List<object> LoadFromStorage()
        //{
        //    using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
        //    {
        //        var dump = JsonSerializer.DeserializeAsync<List<Project>>(fs);
        //    }
        //}

        //public bool WriteToStorage(List<Object>)
        //{
        //    using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
        //    {

        //    }
        //}
    }
}