using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        ///<summary>  This method sorts Person by subclass, calls EntryListView for display
        ///and calls the SortCurrentOrAllProject method
        ///</summary>
        public static void SortByPerson<T>()
        {
            var persons = ((PersonDataTable)App.DB[DBTable.Person]).GetPersonsOfType<T>();

            // Renderer's selectors only accepts [0-9]{1,2} as value
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < persons.Count; i++) conversionTable.Add($"{i}", persons[i].UUID);

            EntryListView ListPage = new EntryListView(persons.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.ToString()}"),
                typeof(T).Name.ToLower()
            );

            var userRequest = App.Renderer.Render(ListPage);

            string uuid = conversionTable[userRequest.GetSelectedValue()];

            Person target = (Person)App.DB[DBTable.Person][uuid];

            SortCurrentOrAllProjects(target);
        }

        /// <summary>
        /// This method sorts all the courses, calls EntryListView for display
        /// and calls the DisplayProject method
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
            List<Project> projects = ((Course)target).Projects.ConvertAll(e => (Project)e).OrderBy(x=>x.Topic).ToList();
            DisplayProjects(projects);
        }

        public static void SortBySchoolYear()
        {
            var schoolyear = App.DB[DBTable.FormYear].Entries.ToList().ConvertAll(entry => (FormYear)entry);

            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < schoolyear.Count; i++) conversionTable.Add($"{i}", schoolyear[i].UUID);

            EntryListView ListPage = new EntryListView(schoolyear.ToDictionary(
                s => conversionTable.First(kvp => kvp.Value.Equals(s.UUID)).Key,
                s => $"{s.GradeName}"),
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
            List<Project> projects = ((Promotion)target).Projects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList();
            DisplayProjects(projects); ;
        }

        public static void SortByDateAsc()
        {
            var projects = App.DB[DBTable.Project].Entries.ToList().ConvertAll(entry => (Project)entry);
            projects = projects.OrderBy(e => e.EndDate).ToList();

            DisplayProjects(projects);
        }

        public static void SortByDateDesc()
        {
            var projects = App.DB[DBTable.Project].Entries.ToList().ConvertAll(entry => (Project)entry);
            projects = projects.OrderBy(e => e.EndDate).Reverse().ToList();

            DisplayProjects(projects);
        }

        public static void SortByKeywords()
        {
            throw new NotImplementedException();
        }

        private static void SortCurrentOrAllProjects(EntryType target)
        {
            
            CurrentOrAllView ListPage = new CurrentOrAllView(target.GetType().Equals(typeof(Student)));
            var input = App.Renderer.Render(ListPage);
            string userRequest = input.GetSelectedValue();

            switch (userRequest)
            {
                case "0"://All projects
                    if (target.GetType().Equals(typeof(Student)) ^ target.GetType().Equals(typeof(Extern)) ^ target.GetType().Equals(typeof(Teacher))) { DisplayProjects(((Person)target).Projects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    else if (target.GetType().Equals(typeof(FormYear))) { DisplayProjects(((FormYear)target).AllProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
                case "1"://Currents projects
                    if (target.GetType().Equals(typeof(Student)) ^ target.GetType().Equals(typeof(Extern)) ^ target.GetType().Equals(typeof(Teacher))) { DisplayProjects(((Person)target).ActiveProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    else if(target.GetType().Equals(typeof(FormYear))) { DisplayProjects(((FormYear)target).CurrentProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
                case "2": // //Current promotion project
                    if (target.GetType().Equals(typeof(Student))) { DisplayProjects(((Student)target).CurrentPromotionProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
            }

        }

        private static void DisplayProjects(List<Project> projects)
        {
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < projects.Count; i++) conversionTable.Add($"{i}", projects[i].UUID);

            ProjectListView ListPage = new ProjectListView(projects.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.Topic}")
            );

            var userRequest = App.Renderer.Render(ListPage);
            if (projects.Count!=0) { string uuid = conversionTable[userRequest.GetSelectedValue()]; }

        }
    }
}
