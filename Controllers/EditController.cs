using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using System;
using System.Net;
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

            while(!saveRequested)
            {
                var setDetailsPage = new NewProjectAdvancedInfoView(newProject);
                var userRequest = App.Renderer.Render(setDetailsPage).GetSelectedValue();

                switch (userRequest)
                {
                    case "0": // add deliverable
                        break;

                    case "1": // add promotion
                        break;

                    case "2": // add course
                        break;

                    case "3": // add contributor
                        break;

                    case "4": // cancel
                        return;

                    case "5": // save
                        saveRequested = true;
                        break;
                }
            }




            //App.DB[DBTable.Project].AddEntry(newProject);
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
    }
}