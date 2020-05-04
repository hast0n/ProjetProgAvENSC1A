using System;
using System.Collections.Generic;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Controllers;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A
{
    class App
    {
        public static DataBase DB = new DataBase();
        public static Renderer console = new Renderer() { FrameWidth = 70 };

        public App()
        {
            DB.Load();

            console.SetDefaultResources();
            console.AddVisualResources(new Dictionary<string, string>()
            {
                {
                    "appName",
                    "--- PROJECT MANAGER ---"
                },
                {
                    "userStatus",
                    "$userStatus"
                }
            });

            //User user = LoginController.Authenticate();

            //SelectorTestView selectorTestView = new SelectorTestView(console.VisualResources, console.SplitChar);

            //console.Render(selectorTestView);
        }

        public void Debug()
        {
            Course c1 = new Course();
            Course c2 = new Course();

            Promotion p1 = new Promotion();

            Person u1 = new Person();
            Person u2 = new Person();
            Person u3 = new Person();

            Project p = new Project()
            {
                Topic = "A brand new project",
                StartDate = new DateTime(2020, 05, 01),
                EndDate = new DateTime(2020, 06, 01),
                Deliverables = new List<Deliverable>()
                {
                    new Deliverable()
                    {
                        Type = TypeDeliverable.ApplicationLogicielle,
                        Information = "I want an app to sort my shoes"
                    },
                    new Deliverable()
                    {
                        Type = TypeDeliverable.Rapport,
                        Information = "Tell me all that I wanna know"
                    }
                },
                Courses = new List<Course>() { c1, c2 },
                Promotions = new List<Promotion>() { p1 },
                Contributors = new List<Person>() { u1, u2, u3 }
            };


            DB[DBTable.Courses].AddEntry(c1);
            DB[DBTable.Courses].AddEntry(c2);

            DB[DBTable.Promotion].AddEntry(p1);

            DB[DBTable.Person].AddEntry(u1);
            DB[DBTable.Person].AddEntry(u2);
            DB[DBTable.Person].AddEntry(u3);
            
            DB[DBTable.Project].AddEntry(p);

            DB.Persist();
        }

        public void Launch()
        {
            throw new NotImplementedException();
        }
    }
}

// TODO: Lock edition to active field
// TODO: Press Return to edit next field
// TODO:    --> add "completed" attribute to modifierDictionary for inputs
// TODO: Empty active field & backspace to access previous one