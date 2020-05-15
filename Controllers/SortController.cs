using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using ProjetProgAvENSC1A.Services.DataTables;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A.Controllers
{
    class SortController
    {
        public static void SortByPerson<T>()
        {
            var persons = ((PersonDataTable)App.DB[DBTable.Person]).GetPersonsOfType<T>();

            // Renderer's selectors only accepts ints as value
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < persons.Count; i++) conversionTable.Add($"{i}", persons[i].UUID);

            EntryListView ListPage = new EntryListView(persons.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.FirstName} {p.LastName}"),
                typeof(T).Name.ToLower()
            );

            var userRequest = App.Renderer.Render(ListPage);

            string uuid = conversionTable[userRequest.GetSelectedValue()];

            Person target = (Person)App.DB[DBTable.Person][uuid];

            DisplayProjects(target);
        }

        public static void SortByCourses()
        {
            throw new NotImplementedException();
        }

        public static void SortBySchoolYear()
        {
            throw new NotImplementedException();
        }

        public static void SortByPromotions()
        {
            throw new NotImplementedException();
        }

        public static void SortByDateAsc()
        {
            throw new NotImplementedException();
        }

        public static void SortByDateDesc()
        {
            throw new NotImplementedException();
        }

        public static void SortByKeywords()
        {
            throw new NotImplementedException();
        }

        private static void DisplayProjects(Person target)
        {
            var projects = target.Projects;

        }
    }
}
