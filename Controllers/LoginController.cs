using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A.Controllers
{
    class LoginController
    {
        public static User Authenticate(int attempts)
        {
            // Launch LoginView
            //var LoginView = new LoginView(App.Renderer.VisualResources, App.Renderer.SplitChar);
            //var result = App.Renderer.Render(LoginView);

            // extract input name
            // extract input password
            // hash password
            // find User with corresponding name input and password hash

            // if ok : return user
            // if nok : return UnauthorizedUser

            return new TestUser();
        }
    }
}
