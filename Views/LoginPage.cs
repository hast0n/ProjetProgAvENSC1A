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
                "topBar",
                "[emptyLine]",
                "[loginHint]",
                "[emptyLine]",
                "[userHint]",
                "[input]",
                "[pswHint]",
                "[input]",
                "[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "loginHint",
                    "Login with your credentials to access content :"
                },
                {
                    "userHint",
                    "Select username :"
                },
                {
                    "pswHint",
                    "Select password"
                },
                {
                    "input",
                    String.Join(splitChar, ""+
                        "┌────────┐",
                        "│<input regex=[A-Za-z0-9] length=8>│",
                        "└────────┘")
                }
            };
        }
    }
}
