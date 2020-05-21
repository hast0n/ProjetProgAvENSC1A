using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views.NewProjectViews
{
    class NewProjectAdvancedInfoView : ContentView
    {
        private Project currentProject;
        private string redCol = "<color value=red>";
        private string blackCol = "<color value=black>";

        public NewProjectAdvancedInfoView(Project p, int errorType)
        {
            currentProject = p;

            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "[emptyLine]",

                // Contributors
                "[contribTitle]",
                "[contribList]",
                "[emptyLine]",
                "[studentAddHint]",
                "[teacherAddHint]",
                "[externAddHint]",
                
                "2*[emptyLine]",
                
                // Deliverables
                "[delivTitle]",
                "[delivList]",
                "[emptyLine]",
                "[delivAddHint]",

                "2*[emptyLine]",
                
                // Promotions
                "[promTitle]",
                "[promList]",
                "[emptyLine]",
                "[promAddHint]",

                "2*[emptyLine]",
                
                // Courses
                "[courseTitle]",
                "[courseList]",
                "[emptyLine]",
                "[courseAddHint]",

                errorType switch
                {
                    1 => "[noPromError]",
                    2 => "[noTeacherError]",
                    _ => "[emptyLine]"
                },

                "3*[emptyLine]",
                "[saveBtn]",
                "1*[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {"delivTitle", "-- Deliverables --"},
                {"delivList", BuildDeliverableList()},
                {"delivAddHint", "<selector value=0 text=' Add a deliverable '>"},

                {"promTitle", "-- Promotions --"},
                {"promList", BuildPromotionList()},
                {"promAddHint", "<selector value=1 text=' Add a promotion '>"},
                
                {"courseTitle", "-- Courses --"},
                {"courseList", BuildCourseList()},
                {"courseAddHint", "<selector value=2 text=' Add a course '>"},
                
                {"contribTitle", "-- Contributors --"},
                {"contribList", BuildContributorsList()},
                {"studentAddHint", "<selector value=3 text=' Add a student '>"},
                {"teacherAddHint", "<selector value=4 text=' Add a teacher '>"},
                {"externAddHint", "<selector value=5 text=' Add an extern '>"},

                {"noPromError", $"{redCol} Please, select a promotion first {blackCol}"},
                {"noTeacherError", $"{redCol} Please, select a teacher first {blackCol}"},

                {
                    "saveBtn",
                    String.Join(App.Renderer.SplitChar, "" +
                        "┌─────────────────────┐",
                        "│ <selector value=6 text=' Cancel and delete ' color=red> │",
                        "│ <selector value=7 text='       Save        ' color=blue> │",
                        "└─────────────────────┘")
                }
            };
        }

        private string BuildCourseList()
        {
            StringBuilder sb = new StringBuilder();

            int count = currentProject.Courses != null
                ? currentProject.Courses.Count
                : 0;

            if (count < 1)
            {
                sb.Append("There are no associated courses...");
            }
            else
            {
                int maxLength = currentProject.Courses.Max(e => e.Name.Length);

                foreach (var c in currentProject.Courses)
                {
                    sb.AppendFormat(" - {0}{1}", c.Name.PadRight(maxLength), App.Renderer.SplitChar);
                }
            }

            return sb.ToString();
        }

        private string BuildPromotionList()
        {
            StringBuilder sb = new StringBuilder();

            int c = currentProject.Promotions != null
                ? currentProject.Promotions.Count
                : 0;

            if (c < 1)
            {
                sb.Append("There are no associated promotions...");
            }
            else
            {
                int maxLength = currentProject.Promotions.Max(p => p.ToString().Length);

                foreach (var p in currentProject.Promotions)
                {
                    sb.AppendFormat(" - {0}{1}", p.ToString().PadRight(maxLength), App.Renderer.SplitChar);
                }
            }

            return sb.ToString();
        }

        private string BuildContributorsList()
        {
            StringBuilder sb = new StringBuilder();

            int c = currentProject.Contributors != null
                ? currentProject.Contributors.Count
                : 0;

            if (c < 1)
            {
                sb.Append("There are no associated contributors...");
            }
            else
            {
                int maxLength = currentProject.Contributors.Max(kvp =>
                    $"{kvp.Key} : {kvp.Value.FullName}".Length);

                foreach (var kvp in currentProject.Contributors)
                {
                    string tmp = $"{kvp.Key} : {kvp.Value.FullName}".PadRight(maxLength);
                    sb.AppendFormat(" - {0}{1}", tmp, App.Renderer.SplitChar);
                }
            }

            return sb.ToString();
        }

        private string BuildDeliverableList()
        {
            StringBuilder sb = new StringBuilder();

            int c = currentProject.Deliverables != null
                ? currentProject.Deliverables.Count
                : 0;

            if (c < 1)
            {
                sb.Append("There are no associated deliverable...");
            }
            else
            {
                int maxLength = currentProject.Deliverables.Max(d => d.Type.ToString().Length);

                foreach (var d in currentProject.Deliverables)
                {
                    string tmp = d.Type.ToString().PadRight(maxLength);
                    sb.AppendFormat(" - {0}{1}", tmp, App.Renderer.SplitChar);
                }
            }

            return sb.ToString();
        }
    }
}
