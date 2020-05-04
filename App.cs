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

            #region define persons (7)
            Teacher t1 = (Teacher) new Person()
            {
                FirstName = "Jean",
                LastName = "David",
                Age = 35,
                Gender = Constants.Gender.Masculin
            };
            
            Teacher t2 = (Teacher) new Person()
            {
                FirstName = "Jeanne",
                LastName = "Paris",
                Age = 31,
                Gender = Constants.Gender.Feminin
            };
            
            Teacher t3 = (Teacher) new Person()
            {
                FirstName = "Arthur",
                LastName = "Pratt",
                Age = 42,
                Gender = Constants.Gender.Autre
            };

            Student s1 = (Student) new Person()
            {
                FirstName = "Perceval",
                LastName = "Caillet",
                Age = 19,
                Gender = Constants.Gender.Masculin
            };

            Student s2 = (Student) new Person()
            {
                FirstName = "Iris",
                LastName = "Fouqueault",
                Age = 21,
                Gender = Constants.Gender.Feminin
            };

            Student s3 = (Student) new Person()
            {
                FirstName = "Emmeline",
                LastName = "Tournot",
                Age = 20,
                Gender = Constants.Gender.Feminin
            };

            Extern e1 = (Extern) new Person()
            {
                FirstName = "Jacques",
                LastName = "Sabatier",
                Age = 28,
                Gender = Constants.Gender.Masculin
            };
            #endregion

            #region define courses (3)
            Course c1 = new Course()
            {
                Name = "Anglais",
                Teachers = new List<Teacher>() { t1 }
            };
            Course c2 = new Course()
            {
                Name = "Géographie",
                Teachers = new List<Teacher>() { t2 }
            };
            Course c3 = new Course()
            {
                Name = "Programmation",
                Teachers = new List<Teacher>() { t3 }
            };
            #endregion

            #region define formYear (1)
            FormYear f1 = new FormYear()
            {
                GradeName = Constants.GradeYear.Y1,
                Courses = new List<Course>() { c1, c2, c3 }
            };
            #endregion

            #region define promotions (1)
            Promotion p1 = new Promotion()
            {
                GraduationYear = 2022,
                Grade = f1,
                Students = new List<Student>() { s1, s2, s3 },
                Name = "Bernoulli"
            };
            #endregion

            #region define projects (1)
            Project proj1 = new Project()
            {
                Topic = "Projet de programmation",
                StartDate = new DateTime(2020, 05, 01),
                EndDate = new DateTime(2020, 06, 01),
                Deliverables = new List<Deliverable>()
                {
                    new Deliverable()
                    {
                        Type = TypeDeliverable.ApplicationLogicielle,
                        Information = "Je veux une application pour trier mes chaussettes"
                    },
                    new Deliverable()
                    {
                        Type = TypeDeliverable.Rapport,
                        Information = "Dis moi tout ce que tu sais à propos du Cloud Computing"
                    }
                },
                Courses = new List<Course>() { c3 },
                Promotions = new List<Promotion>() { p1 },
                Contributors = new List<Person>() { s1, s2, e1 }
            };
            #endregion

            foreach (var pers in new List<Person>() {t1, t2, t3, s1, s2, s3, e1})
                DB[DBTable.Person].AddEntry(pers);
            
            DB[DBTable.FormYear].AddEntry(f1);

            DB[DBTable.Courses].AddEntry(c1);
            DB[DBTable.Courses].AddEntry(c2);
            DB[DBTable.Courses].AddEntry(c3);

            DB[DBTable.Promotion].AddEntry(p1);
            
            DB[DBTable.Project].AddEntry(proj1);

            // write on storage
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