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
        ///<summary>  This method sorts Person by subclass in a view
        ///</summary>
        public static void SortByPerson<T>()
        {
            var persons = ((PersonDataTable)App.DB[DBTable.Person]).GetPersonsOfType<T>();

            // Renderer's selectors only accepts [0-9]{1,2} as value
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

            SortCurrentOrAllProjects<Person>(target);
            //DisplayProjects(target);
        }

        /// <summary>
        /// This method presents all the courses
        /// </summary>
        public static void SortByCourses()
        {
            var courses = App.DB[DBTable.Courses].Entries.ToList().ConvertAll(entry => (Course)entry);

            Dictionary<string, string> conversionTable = new Dictionary<string, string>();
            
            for (int i = 0; i < courses.Count; i++) conversionTable.Add($"{i}", courses[i].UUID);

            EntryListView ListPage = new EntryListView(courses.ToDictionary(
                c => conversionTable.First(kvp => kvp.Value.Equals(c.UUID)).Key,
                c => $"{c.Name} with {c.TeachersToString()}"),
                "course"
            );

            var userRequest = App.Renderer.Render(ListPage);

            string uuid = conversionTable[userRequest.GetSelectedValue()];

            Course target = (Course)App.DB[DBTable.Courses][uuid];

            DisplayProjects(target);
        }

        public static void SortBySchoolYear()
        {
            var schoolyear = App.DB[DBTable.FormYear].Entries.ToList().ConvertAll(entry => (FormYear)entry);

            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < schoolyear.Count; i++) conversionTable.Add($"{i}", schoolyear[i].UUID);

            EntryListView ListPage = new EntryListView(schoolyear.ToDictionary(
                s => conversionTable.First(kvp => kvp.Value.Equals(s.UUID)).Key,
                s => $"{s.GradeName}"), // Peut-être changer en 1A, 2A,3A
                "school year"
            );

            var userRequest = App.Renderer.Render(ListPage);

            string uuid = conversionTable[userRequest.GetSelectedValue()];

            FormYear target = (FormYear)App.DB[DBTable.FormYear][uuid];

            SortCurrentOrAllProjects<FormYear>(target);
            //DisplayProjects(target);
        }

        public static void SortByPromotions()
        {
            var promo = App.DB[DBTable.Promotion].Entries.ToList().ConvertAll(entry => (Promotion)entry);

            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < promo.Count; i++) conversionTable.Add($"{i}", promo[i].UUID);

            EntryListView ListPage = new EntryListView(promo.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.Name} {p.GraduationYear} {p.Grade.GradeName}"), // 
                "promotion"
            );

            var userRequest = App.Renderer.Render(ListPage);

            string uuid = conversionTable[userRequest.GetSelectedValue()];

            Promotion target = (Promotion)App.DB[DBTable.Promotion][uuid];
            DisplayProjects(target);
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

        //To do : 
        // 1 . Interface all or current projects 
        // 2 . join those functions
        private static void  SortCurrentOrAllProjects<T>(EntryType target)
        {
            
            CurrentOrAllView ListPage = new CurrentOrAllView();
            var input = App.Renderer.Render(ListPage);
            string userRequest = input.GetSelectedValue();

            switch (userRequest)
            {
                case "0"://Currents projects
                    if (typeof(T) is Person){ DisplayProjects((Person)target, true); }
                    else if(typeof(T) is FormYear) { DisplayProjects((FormYear)target, true); }
                    break;
                case "1":
                    if (typeof(T) is Person) { DisplayProjects((Person)target, false); }
                    else if (typeof(T) is FormYear) { DisplayProjects((FormYear)target, false); }
                    break;
                //default:
                //    App.Launch();
                //    break;
            }

        }
        private static void DisplayProjects(EntryType target, bool booleen)
        {
            var bo = 2;
            //var projects = target.Projects;
        }
        private static void DisplayProjects(Course target)
        {
            var projects = target.Projects;
        }
        private static void DisplayProjects(FormYear target, bool booleen)
        {
            if (booleen) { var projects = target.AllProjects; }
            else { var projects = target.CurrentProjects; }
        }
        private static void DisplayProjects(Promotion target)
        {
            var projects = target.Projects;
        }
    }
}
