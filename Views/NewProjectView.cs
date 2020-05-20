using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views
{
    class NewProjectView : ContentView
    {
        public NewProjectView(int errorType = 0)
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "1*[emptyLine]",

                "[topicInput]",
                "[emptyLine]",

                "[dateInput]",
                "[emptyLine]",

                "[dateInput]",
                "[emptyLine]",
                
                "[emptyLine]",
                
                "[emptyLine]",

                "5*[emptyLine]",

                errorType switch
                {
                    1 => "[startDateError]",
                    2 => "[endDateError]",
                    3 => "[dateError]",
                    4 => "[dateError]",
                    _ => "[emptyLine]"
                },

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
                    "topicInput",
                    String.Join(App.Renderer.SplitChar, ""+ 
                        "Give a nice title to the new project below:",
                        $"┌{new string('─', 60 + 2)}┐",
                        "│ <input regex='[\\w0-9 ]' length=60> │",
                        $"└{new string('─', 60 + 2)}┘")
                },
                {
                    "dateInput",
                    String.Join(App.Renderer.SplitChar, ""+
                        " DD   MM   YYYY",
                        "┌──┐ ┌──┐ ┌────┐",

                        "│<input regex='[0-9]' length=2>│/" +
                        "│<input regex='[0-9]' length=2>│/" +
                        "│<input regex='[0-9]' length=4>│",

                        "└──┘ └──┘ └────┘")
                },
                {
                    "hitEnterHint",
                    "<color value=white> Fill in the project's details and press Enter to continue... <color value=black>"
                },
            };

        }
    }
}