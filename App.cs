using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjetProgAvENSC1A.Controllers;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A
{
    class App
    {
        public static DataBase DB = new DataBase();
        public static Renderer Renderer = new Renderer() { FrameWidth = 70 };

        public readonly User UnauthorizedUser = new User()
        {
            Privilege = Privilege.Unauthorized,
        };

        private User _user;
        public string Username => _user.Name;
        public Privilege UserPrivilege => _user.Privilege;

        public App()
        {
            DB.Load();

            _user = UnauthorizedUser;

            Renderer.SetDefaultResources();
            Renderer.AddVisualResources(new Dictionary<string, string>()
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
        }

        public void Debug(bool writeToStorage)
        {
            #region define persons (7)
            Teacher t1 = new Teacher()
            {
                FirstName = "Jean",
                LastName = "David",
                Age = 35,
                Gender = Constants.Gender.Masculin
            };

            Teacher t2 = new Teacher()
            {
                FirstName = "Jeanne",
                LastName = "Paris",
                Age = 31,
                Gender = Constants.Gender.Feminin
            };
            
            Teacher t3 = new Teacher()
            {
                FirstName = "Arthur",
                LastName = "Pratt",
                Age = 42,
                Gender = Constants.Gender.Autre
            };

            Student s1 = new Student()
            {
                FirstName = "Perceval",
                LastName = "Caillet",
                Age = 19,
                Gender = Constants.Gender.Masculin,
                Student_ID = "pcaillet"
            };

            Student s2 = new Student()
            {
                FirstName = "Iris",
                LastName = "Fouqueault",
                Age = 21,
                Gender = Constants.Gender.Feminin,
                Student_ID = "ifouqueault"
            };

            Student s3 = new Student()
            {
                FirstName = "Emmeline",
                LastName = "Tournot",
                Age = 20,
                Gender = Constants.Gender.Feminin,
                Student_ID = "etournot"
            };

            Extern e1 = new Extern()
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
                Contributors = new Dictionary<Role, Person>()
                {
                    {
                        Role.ChefDeProj,
                        s1
                    },
                    {
                        Role.Developp,
                        s2
                    },
                    {
                        Role.Secretaire,
                        s3
                    },
                }
            };
            #endregion

            #region define users (1)
            User u1 = new User()
            {
                Name = "admin",
                Privilege = Privilege.Administrator,
                PasswordHash = SHA.GenerateSHA512String("Bonjour")
            };
            
            User u2 = new User()
            {
                Name = "toto",
                Privilege = Privilege.Spectator,
                PasswordHash = SHA.GenerateSHA512String("salut")
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
            
            DB[DBTable.User].AddEntry(u1);
            DB[DBTable.User].AddEntry(u2);

            // write on storage
            if (writeToStorage) DB.Persist();
        }

        public void Launch()
        {
            _user = LoginController.Authenticate();

            while (!_user.Privilege.Equals(Privilege.Unauthorized))
            {
                _user = LoginController.Authenticate();
            }

            while (Route())
            {
                //
            }
        }

        public bool Route()
        {
            //HomeView homeView = new HomeView();


            //SelectorTestView selectorTestView = new SelectorTestView(console.VisualResources, console.SplitChar);

            //console.Render(selectorTestView);

            return true;
        }
    }
}

// TODO: Lock edition to active field
// TODO: Press Return to edit next field
// TODO:    --> add "completed" attribute to modifierDictionary for inputs
// TODO: Empty active field & backspace to access previous one