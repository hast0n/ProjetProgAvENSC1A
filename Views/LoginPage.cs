using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class LoginPage : ContentPage
    {
        public LoginPage(
            Dictionary<string, string> sharedResources,
            char splitChar)
        {
            SharedResources = sharedResources;


            Layout = new List<string>()
            {
                "2*emptyLine",
                "topBar",
                "2*[emptyLine]",
                "[loginHint]",
                "2*[emptyLine]",
                "[userHint]",
                "[userInput]",
                "[emptyLine]",
                "[pswHint]",
                "[pswInput]",
                "2*[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "loginHint",
                    "Login with your credentials " +
                    "to access the application:"
                },
                {
                    "userHint",
                    "Enter Username :"
                },
                {
                    "pswHint",
                    "Enter Password :"
                },
                {
                    "userInput",
                    String.Join(splitChar, ""+
                        "┌────────────┐",
                        "│  <input regex=[A-Za-z0-9\\s] length=8>  │",
                        "└────────────┘")
                },
                {
                    "pswInput",
                    String.Join(splitChar, ""+
                        "┌────────────┐",
                        "│  <input regex=[A-Za-z0-9] length=8>  │",
                        "└────────────┘")
                }
            };
        }
    }
}