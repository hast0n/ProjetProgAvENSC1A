using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetProgAvENSC1A.Views
{
    class ProjectView : ContentView
    {
        public ProjectView(Project entry)
        {
            SharedResources = App.Renderer.VisualResources;
            bool existingContributor = (entry.Contributors.Count > 0);
            bool existingCourse = (entry.Courses.Count > 0);
            bool existingDate = entry.EndDate != null & entry.StartDate != null;
            bool existingDeliverable = (entry.Deliverables.Count > 0);
            bool existingPromo = (entry.Promotions.Count > 0);
            
            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",

                "[title]",
                "[emptyLine]",
                existingCourse ? "[Courses]" : "[emptyLine]",
                existingPromo ? "[Promotions]" : "[emptyLine]",
                existingDate ? "[Dates]" : "[emptyLine]",
                "[emptyLine]",
                existingContributor ? "[Contributors]" : "[emptyLine]",
                "[emptyLine]",
                existingDeliverable ? "[Deliverables]":"[emptyLine]",

                "3*[emptyLine]",
                "botBar",
                "pressAnyHint"
            };           
            
            
            LocalResources = new Dictionary<string, string>()
            {
                {
                    "title",
                    $"{entry.Topic}"
                },
                {
                    "Contributors",
                     GetFormattedContributors(entry)
                },
                 {
                    "Courses",
                     GetFormattedCourses(entry)
                },
                {
                    "Dates",
                    $"Between {entry.StartDate.ToShortDateString()} and {entry.EndDate.ToShortDateString()} : {entry.Duration.Days} days"
                },
                {
                    "Deliverables",
                    GetFormattedDelivrables(entry)
                },
                {
                    "Promotions",
                    GetFormattedPromotions(entry)
                }
            };

        }
        public string GetFormattedContributors(Project entry)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.AppendLine("with :");

            int maxLengthPerson = entry.Contributors.Values.Max(e => e.FullName.Length);
            int maxLengthRole = entry.Contributors.Keys.Max(e => e.ToString().Length);

            foreach (var kvp in entry.Contributors)
            {
                sb.AppendFormat("- {0} : {1} {2}",
                    kvp.Key.ToString().PadLeft(maxLengthRole), kvp.Value.FullName.PadLeft(maxLengthPerson), App.Renderer.SplitChar);
            }

            return sb.ToString();
        }
        public string GetFormattedCourses(Project entry)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append("Course(s) : ");

            foreach (var course in entry.Courses)
            {
                sb.AppendFormat("{0} ,", course.Name);
            }

            return sb.ToString();
        }
        public string GetFormattedDelivrables(Project entry)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append("Delivrable(s) : \n");

            foreach (var deliv in entry.Deliverables)
            {
                sb.AppendFormat("- {0} : {1} {2}",
                    deliv.Type.ToString(), deliv.Information, App.Renderer.SplitChar);
            }

            return sb.ToString();
        }
        public string GetFormattedPromotions(Project entry)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append("Group(s) of students : ");

            foreach (var prom in entry.Promotions)
            {
                sb.AppendFormat("{0} Promotion {1} ,",
                    prom.Grade.GradeName, prom.GraduationYear);
            }

            return sb.ToString();
        }
    }
}
