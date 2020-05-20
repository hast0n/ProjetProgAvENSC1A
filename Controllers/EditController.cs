using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using System;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A.Controllers
{
    class EditController
    {
        public static void AddNewProject()
        {
            var newProjectPage = new NewProjectView();
            var userRequest = App.Renderer.Render(newProjectPage);



            //var credentials = userRequest.GetUserInputs();

            //string nameRequest = credentials[0];
            //string hashRequest = SHA.GenerateSHA512String(credentials[1]);

            //User user = (User)App.DB[DBTable.User].Entries.Find(entry =>
            //{
            //    User u = (User)entry;
            //    return u.PasswordHash.Equals(hashRequest) && u.Name.Equals(nameRequest);
            //});

            //return user ?? new User();
        }

        public static void RemoveProject()
        {
            throw new NotImplementedException();
        }
    }
}