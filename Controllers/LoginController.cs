using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Services;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A.Controllers
{
    class LoginController
    {
        public static User Authenticate(int attempts)
        {
            //var LoginView = new LoginView(attempts);
            //var userRequest = App.Renderer.Render(LoginView);

            //var credentials = userRequest.GetUserInputs();

            //string nameRequest = credentials[0];
            //string hashRequest = SHA.GenerateSHA512String(credentials[1]);

            //User user = (User)App.DB[DBTable.User].Entries.Find(entry =>
            //{
            //    User u = (User) entry;
            //    return u.PasswordHash.Equals(hashRequest) && u.Name.Equals(nameRequest);
            //});

            //return user ?? new User();
            return new User()
            {
                Name = "TestUser",
                Privilege = Privilege.Administrator
            };
        }
    }
}
