using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services.DataTables;

namespace ProjetProgAvENSC1A.Services
{
    public enum DBTable
    {
        Courses,
        FormYear,
        Person,
        Project,
        Promotion,
        User
    }

    class DataBase
    {
        private Dictionary<DBTable, IDataTable> _data;
        public IDataTable this[DBTable tableIndex] => _data[tableIndex];

        public DataBase()
        {
            // instantiate database tables
            _data = new Dictionary<DBTable, IDataTable>()
            {
                {DBTable.Project, new ProjectDataTable()},
                {DBTable.Courses, new CourseDataTable()},
                {DBTable.FormYear, new FormYearDataTable()},
                {DBTable.Person, new PersonDataTable()},
                {DBTable.Promotion, new PromotionDataTable()},
                //{DBTable.User, new UserDataTable()},
            };
        }

        public bool Load()
        {
            Directory.CreateDirectory("data"); // check if folder exists : if not, recreate it
            return new IDataTable[]
            {   // order matters
                _data[DBTable.Person],
                _data[DBTable.Courses],
                _data[DBTable.FormYear],
                _data[DBTable.Promotion],
                _data[DBTable.Project],
            }.All(dt => dt.LoadFromStorage().Result);
        }

        public bool Persist()
        {
            Directory.CreateDirectory("data"); // check if folder still exists : if not, recreate it
            return _data.All(keyValuePair => keyValuePair.Value.WriteToStorage().Result); // order doesn't matter
        } 
    }
}
