using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using ProjetProgAvENSC1A.Views;
using ProjetProgAvENSC1A.Views.NewProjectViews;

namespace ProjetProgAvENSC1A.Controllers
{
    class EditController
    {
        public static void AddNewProject()
        {
            Project newProject = AskBasicInfo();
            
            bool saveRequested = false;
            int errorType = 0;

            while (!saveRequested)
            {
                var setDetailsPage = new NewProjectAdvancedInfoView(newProject, errorType);
                var userRequest = App.Renderer.Render(setDetailsPage).GetSelectedValue();

                Person contrib;
                Role role;

                switch (userRequest)
                {
                    case "0": // add deliverable
                        var deliv = AskDeliverable();
                        newProject.Deliverables.Add(deliv);
                        break;

                    case "1": // add promotion
                        var prom = AskPromotion();

                        if (!newProject.Promotions.Contains(prom))
                            newProject.Promotions.Add(prom);

                        break;

                    case "2": // add course
                        var course = AskCourse();
                        newProject.Courses.Add(course);

                        break;

                    case "3": // add student

                        if (newProject.Promotions.Count < 1)
                        {
                            errorType = 1;
                            continue;
                        }

                        contrib = AskContributor(newProject, "student");
                        role = AskContribRole(contrib);

                        if (!newProject.Contributors.Values.Contains(contrib)
                            && !newProject.Contributors.Keys.Contains(role))
                            newProject.Contributors.Add(role, contrib);
                        else
                        {
                            errorType = 3;
                        }
                        
                        break;

                    case "4": // add teacher

                        contrib = AskContributor(newProject, "teacher");
                        role = AskContribRole(contrib);

                        if (!newProject.Contributors.Values.Contains(contrib)
                            && !newProject.Contributors.Keys.Contains(role))
                            newProject.Contributors.Add(role, contrib);
                        else
                        {
                            errorType = 3;
                        }

                        break;

                    case "5": // add extern

                        contrib = AskContributor(newProject, "extern");
                        role = AskContribRole(contrib);

                        if (!newProject.Contributors.Values.Contains(contrib)
                            && !newProject.Contributors.Keys.Contains(role))
                            newProject.Contributors.Add(role, contrib);
                        else
                        {
                            errorType = 3;
                        }

                        break;

                    case "6": // cancel
                        return;

                    case "7": // save
                        saveRequested = true;
                        break;
                }
            }

            App.DB[DBTable.Project].AddEntry(newProject);
            App.DB.Persist();
        }

        public static void RemoveProject()
        {
            throw new NotImplementedException();
        }

        private static Project AskBasicInfo()
        {
            bool valid = false;

            Project newProject = new Project();

            int errorType = 0;

            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.MaxValue;

            string topic = String.Empty;

            while (!valid)
            {
                var newProjectPage = new NewProjectBasicInfoView(errorType);
                var userRequest = App.Renderer.Render(newProjectPage);

                var details = userRequest.GetUserInputs();

                if (details[0].Length < 1)
                {
                    errorType = 3;
                    continue;
                }

                topic = details[0];

                try
                {
                    startDateTime = new DateTime(
                        int.Parse(details[1]),
                        int.Parse(details[2]),
                        int.Parse(details[3]));
                }
                catch (Exception) // No need to sort what error occured --> wrong date format
                {
                    errorType = 1;
                    continue;
                }

                try
                {
                    endDateTime = new DateTime(
                        int.Parse(details[4]),
                        int.Parse(details[5]),
                        int.Parse(details[6]));
                }
                catch (Exception)
                {
                    errorType = 2;
                    continue;
                }

                if (endDateTime.Subtract(startDateTime).TotalDays < 1)
                {
                    errorType = 4;
                    continue;
                }

                valid = true;
            }

            newProject.Topic = topic;
            newProject.StartDate = startDateTime;
            newProject.EndDate = endDateTime;

            return newProject;
        }
        
        private static Promotion AskPromotion()
        {
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            var promotions = App.DB[DBTable.Promotion].Entries.ConvertAll(x => (Promotion) x);

            for (int i = 0; i < promotions.Count; i++) conversionTable.Add($"{i}", promotions[i].UUID);

            EntryListView promPage = new EntryListView(promotions.ToDictionary(
                    p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                    p => $"{p}"),
                "promotion"
            );

            var promRequested = App.Renderer.Render(promPage).GetSelectedValue();

            string uuid = conversionTable[promRequested];

            return (Promotion)App.DB[DBTable.Promotion][uuid];
        }

        private static Deliverable AskDeliverable()
        {
            var delivTypes = Enum.GetValues(typeof(DeliverableType)).Cast<DeliverableType>();

            EntryListView rolePage = new EntryListView(delivTypes.ToDictionary(
                    r => r.ToString("D"),
                    r => r.ToString("G")),
                "deliverable type"
            );

            var typeRequested = App.Renderer.Render(rolePage).GetSelectedValue();

            var delivInfoRequest = App.Renderer.Render(new NewDeliverableView());
            
            return new Deliverable()
            {
                Type = (DeliverableType)int.Parse(typeRequested),
                Information = delivInfoRequest.GetUserInputs()[0]
            };
        }

        private static Course AskCourse()
        {
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            var courses = App.DB[DBTable.Courses].Entries.ConvertAll(x => (Course)x);

            for (int i = 0; i < courses.Count; i++) conversionTable.Add($"{i}", courses[i].UUID);

            EntryListView coursePage = new EntryListView(courses.ToDictionary(
                    c => conversionTable.First(kvp => kvp.Value.Equals(c.UUID)).Key,
                    c => c.Name),
                "course"
            );

            var courseRequested = App.Renderer.Render(coursePage).GetSelectedValue();

            string uuid = conversionTable[courseRequested];

            return (Course)App.DB[DBTable.Courses][uuid];
        }

        private static Person AskContributor(Project project, string personType)
        {
            Dictionary<string, string> conversionTable = new Dictionary<string, string>();

            List<Person> persons = new List<Person>();

            switch (personType)
            {
                case "student":
                    project.Promotions.ForEach(prom => persons.AddRange(prom.Students));
                    break;

                case "teacher":
                    persons.AddRange(App.DB[DBTable.Person].Entries
                        .Where(entry => ((Person)entry) is Teacher).ToList()
                        .ConvertAll(p => (Teacher)p));
                    break;

                case "extern":
                    persons.AddRange(App.DB[DBTable.Person].Entries
                        .Where(entry => ((Person)entry) is Extern).ToList()
                        .ConvertAll(e => (Extern)e));
                    break;
            }
            
            for (int i = 0; i < persons.Count; i++) conversionTable.Add($"{i}", persons[i].UUID);

            EntryListView contribPage = new EntryListView(persons.ToDictionary(
                    p => conversionTable.First(kvp => kvp.Value.Equals(p.UUID)).Key,
                    p => $"{p}"), 
                personType
            );

            var contribRequested = App.Renderer.Render(contribPage).GetSelectedValue();

            string uuid = conversionTable[contribRequested];

            return (Person)App.DB[DBTable.Person][uuid];
        }

        private static Role AskContribRole(Person contrib)
        {
            var roles = Enum.GetValues(typeof(Role)).Cast<Role>();

            EntryListView rolePage = new EntryListView(roles.ToDictionary(
                    r => r.ToString("D"),
                    r => r.ToString("G")),
                "role"
            );

            var roleRequested = App.Renderer.Render(rolePage).GetSelectedValue();

            return (Role) int.Parse(roleRequested);
        }
    }
}