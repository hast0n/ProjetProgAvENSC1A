using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views
{
    class LoginView : ContentView
    {
        public LoginView(int attempts) 
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {

                "3*emptyLine",
                "appName",
                "2*emptyLine",
                
                "topBar",
                "4*[emptyLine]",
                
                "[loginHint]",
                "2*[emptyLine]",
                
                attempts > 1 ? "[tryAgainHint]" : "[emptyLine]",
                "[emptyLine]",

                "[userInput]",
                "[pswInput]",
                
                "5*[emptyLine]",
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
                    String.Join(App.Renderer.SplitChar, ""+
                        "                   ┌──────────────────────┐",
                        "Enter Username :   │ <input regex=`[\\w0-9 ]` length=20> │",
                        "                   └──────────────────────┘")
                },
                {
                    "pswInput",
                    String.Join(App.Renderer.SplitChar, ""+ 
                        "                   ┌──────────────────────┐",
                        "Enter Password :   │ <input regex=`[\\w0-9 ]` length=20 hidden=true> │",
                        "                   └──────────────────────┘")
                },
                {
                    "hitEnterHint",
                    "<color value=white> Fill in your credentials and press Enter to validate... <color value=black>"
                },
                {
                    "tryAgainHint",
                    "<color value=red>Your credentials didn't match our records. Please try again...<color value=black>"
                }
            };
        }
    }
}