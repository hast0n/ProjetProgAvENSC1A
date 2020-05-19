using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class LoginView : ContentView
    {
        public LoginView(
            Dictionary<string, string> sharedResources,
            char splitChar) : base(sharedResources)
        {
            Layout = new List<string>()
            {

                "3*emptyLine",
                "appName",
                "2*emptyLine",
                
                "topBar",
                "5*[emptyLine]",
                
                "[loginHint]",
                "2*[emptyLine]",
                
                "[userInput]",
                "[pswInput]",
                
                "2*[emptyLine]",
                "[hitEnterHint]",

                "5*[emptyLine]",
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
                        "                   ┌─────────────────┐",
                        "Enter Username :   │ <input regex='[\\w0-9 ]' length=15> │",
                        "                   └─────────────────┘")
                },
                {
                    "pswInput",
                    String.Join(splitChar, ""+
                        "                   ┌─────────────────┐",
                        "Enter Password :   │ <input regex='[\\w0-9 ]' length=15 hidden=true> │",
                        "                   └─────────────────┘")
                },
                {
                    "hitEnterHint",
                    "<color value=white> Fill in your credentials ans press Enter to validate... <color value=black>"
                }
            };
        }
    }
}