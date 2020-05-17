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
        ///and send you to the Current or All project view
        ///</summary>
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

            SortCurrentOrAllProjects(target);
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

            DisplayProjects(target,false);
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

            SortCurrentOrAllProjects(target);
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
            DisplayProjects(target, false);
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

        
        private static void  SortCurrentOrAllProjects(EntryType target)
        {
            
            CurrentOrAllView ListPage = new CurrentOrAllView();
            var input = App.Renderer.Render(ListPage);
            string userRequest = input.GetSelectedValue();

            switch (userRequest)
            {
                case "0"://Currents projects
                    if (target.GetType().Equals(typeof(Person))) { DisplayProjects((Person)target, true); }
                    else if(target.GetType().Equals(typeof(FormYear))) { DisplayProjects((FormYear)target, true); }
                    break;
                case "1":
                    if (target.GetType().Equals(typeof(Person))) { DisplayProjects((Person)target, false); }
                    else if (target.GetType().Equals(typeof(FormYear))) { DisplayProjects((FormYear)target, false); }
                    break;
                //default:
                //    App.Launch();
                //    break;
            }

        }
        private static void DisplayProjects(EntryType target, bool current)
        {
            List<Project> projects = new List<Project>();
            if (current)
            {
                if (target.GetType().Equals(typeof(Student))) { projects = ((Student)target).CurrentPromotionProjects.ConvertAll(e=>(Project)e); }
                else if (target.GetType().Equals(typeof(Extern))) { projects = ((Extern)target).CurrentProjects.ConvertAll(e => (Project)e); }
                else if (target.GetType().Equals(typeof(Teacher))) { projects = ((Teacher)target).CurrentProjects.ConvertAll(e => (Project)e); }
                else if (target.GetType().Equals(typeof(FormYear))) { projects =((FormYear)target).CurrentProjects.ConvertAll(e => (Project)e); }
            }
            else
            {
                if (target.GetType().Equals(typeof(Person))) { projects = ((Person)target).Projects.ConvertAll(e => (Project)e); }
                else if (target.GetType().Equals(typeof(Course))) { projects = ((Course)target).Projects.ConvertAll(e => (Project)e); }
                else if (target.GetType().Equals(typeof(FormYear))) { projects = ((FormYear)target).AllProjects.ConvertAll(e => (Project)e); }
                else if (target.GetType().Equals(typeof(Promotion))) { projects = ((Promotion)target).Projects.ConvertAll(e => (Project)e); }
            }

            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < projects.Count; i++) conversionTable.Add($"{i}", projects[i].UUID);

            EntryListView ListPage = new EntryListView(projects.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.Topic}"), 
                "topic"
            );

            var userRequest = App.Renderer.Render(ListPage);

        }
    }
}
