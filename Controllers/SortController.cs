using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using ProjetProgAvENSC1A.Services.DataTables;
using ProjetProgAvENSC1A.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetProgAvENSC1A.Controllers
{
    class SortController
    {
        ///<summary>  This method sorts Person by subclass into a list, and displays it by calling a view
        ///Calls the method for the next sorting: SortCurrentOrAllProject method
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
        /// This method sorts all the courses into a list, and displays it by calling a view
        /// Calls the  method for the next sorting: DisplayProject method
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
            DisplayProjectList(projects);
        }

        ///<summary>  This method sorts SchoolYear data into a list and displays it by calling a view
        ///Calls the  method for the next sorting: SortCurrentOrAllProject method
        ///</summary>
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

        ///<summary>  This method sorts all Promotions into a list and displays it by calling a view
        ///Calls the  method for the next sorting: SortCurrentOrAllProject method
        ///</summary>
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
            DisplayProjectList(projects); ;
        }

        ///<summary>  This method sorts all Projects by dates (oldest to newest)
        ///and calls the  method for the display
        ///</summary>
        public static void SortByDateAsc()
        {
            var projects = App.DB[DBTable.Project].Entries.ToList().ConvertAll(entry => (Project)entry);
            projects = projects.OrderBy(e => e.EndDate).ToList();

            DisplayProjectList(projects);
        }

        ///<summary>  This method sorts all Projects by dates (newest to oldest)
        ///and calls the  method for the display
        ///</summary>
        public static void SortByDateDesc()
        {
            var projects = App.DB[DBTable.Project].Entries.ToList().ConvertAll(entry => (Project)entry);
            projects = projects.OrderBy(e => e.EndDate).Reverse().ToList();

            DisplayProjectList(projects);
        }

        ///<summary>  This method sorts all projects based on the input
        ///and calls the  method for the display
        ///</summary>
        public static void SortByKeywords()
        {
            var searchView = new SearchView();
            var userRequest = App.Renderer.Render(searchView);

            var inputs = userRequest.GetUserInputs();
            string keyword = inputs[0].ToLower();

            var keywordProjects = App.DB[DBTable.Project].Entries.Where(entry =>
            {
                Project p = (Project)entry;
                return p.Topic.ToLower().Contains(keyword);
            }).ToList().ConvertAll(e=>(Project)e);

    
            DisplayProjectList(keywordProjects);
        }

        ///<summary>  This method sorts the projects with three options : all, current, promotion
        ///and calls the  method for the display
        ///</summary>
        private static void SortCurrentOrAllProjects(EntryType target)
        {
            
            CurrentOrAllView ListPage = new CurrentOrAllView(target.GetType().Equals(typeof(Student)));
            var input = App.Renderer.Render(ListPage);
            string userRequest = input.GetSelectedValue();

            switch (userRequest)
            {
                case "0"://All projects
                    if (target.GetType().Equals(typeof(Student)) ^ target.GetType().Equals(typeof(Extern)) ^ target.GetType().Equals(typeof(Teacher))) { DisplayProjectList(((Person)target).Projects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    else if (target.GetType().Equals(typeof(FormYear))) { DisplayProjectList(((FormYear)target).AllProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
                case "1"://Currents projects
                    if (target.GetType().Equals(typeof(Student)) ^ target.GetType().Equals(typeof(Extern)) ^ target.GetType().Equals(typeof(Teacher))) { DisplayProjectList(((Person)target).ActiveProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    else if(target.GetType().Equals(typeof(FormYear))) { DisplayProjectList(((FormYear)target).CurrentProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
                case "2": // //Current promotion project
                    if (target.GetType().Equals(typeof(Student))) { DisplayProjectList(((Student)target).CurrentPromotionProjects.ConvertAll(e => (Project)e).OrderBy(x => x.Topic).ToList()); }
                    break;
            }

        }

        ///<summary>  This method displays the projects selected into a list
        ///</summary>
        private static void DisplayProjectList(List<Project> projects)
        {
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            for (int i = 0; i < projects.Count; i++) conversionTable.Add($"{i}", projects[i].UUID);

            ProjectListView ListPage = new ProjectListView(projects.ToDictionary(
                p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                p => $"{p.Topic}")
            );

            var userRequest = App.Renderer.Render(ListPage);
            if (projects.Count!=0) 
            { 
                string uuid = conversionTable[userRequest.GetSelectedValue()];
                Project projectTarget = (Project)App.DB[DBTable.Project][uuid];
                DisplayProject(projectTarget);

            }

        }

        ///<summary>  This method call a view in order to display the project selected
        ///</summary>
        private static void DisplayProject(Project project)
        {
            ProjectView TargetPage = new ProjectView(project);
            var userRequest = App.Renderer.Render(TargetPage);
        }
    }
}
