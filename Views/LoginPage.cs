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
            char splitChar) : base(sharedResources)
        {
            Layout = new List<string>()
            {

                "3*emptyLine",
                "appName",
                "2*emptyLine",
                
                "topBar",
                "2*[emptyLine]",
                
                "[loginHint]",
                "2*[emptyLine]",
                
                "[userInput]",
                "[pswInput]",
                
                "2*[emptyLine]",
                "[hitEnterHint]",

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
                    "userInput",
                    String.Join(splitChar, ""+
                        "                   ┌────────────┐",
                        "Enter Username :   │  <input regex=[A-Za-z0-9\\s] length=8>  │",
                        "                   └────────────┘")
                },
                {
                    "pswInput",
                    String.Join(splitChar, ""+
                        "                   ┌────────────┐",
                        "Enter Password :   │  <input regex=[A-Za-z0-9] length=8>  │",
                        "                   └────────────┘")
                },
                {
                    "hitEnterHint",
                    "Press Enter to continue..."
                }
            };
        }
    }
}