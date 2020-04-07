using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Controllers
{
    class LoginController
    {
        private Renderer _console;

        public LoginController(Renderer renderer)
        {
            _console = renderer;
        }

        public static User Authenticate()
        {
            return new User();
        }
    }
}
