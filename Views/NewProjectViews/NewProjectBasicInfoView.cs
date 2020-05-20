using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views
{
    class NewProjectBasicInfoView : ContentView
    {
        public NewProjectBasicInfoView(int errorType)
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "[emptyLine]",

                "[topicInput]",
                "[emptyLine]",

                "[startDateHint]",
                "[dateInput]",
                "[emptyLine]",

                "[endDateHint]",
                "[dateInput]",
                "[emptyLine]",
                
                "2*[emptyLine]",

                errorType switch
                {
                    1 => "[startDateError]",
                    2 => "[endDateError]",
                    3 => "[topicError]",
                    4 => "[negDurationError]",
                    _ => "[hitEnterHint]"
                },

                "2*[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "startDateHint", "Type in the start date of the project:"
                },
                {
                    "endDateHint", "Type in the end date of the project:"
                },
                {
                    "startDateError",
                    "<color value=red>" +
                    " The starting date was not in a correct format! Please try again... " +
                    "<color value=black>"
                },
                {
                    "endDateError",
                    "<color value=red>" +
                    " The ending date was not in a correct format! Please try again... " +
                    "<color value=black>"
                },
                {
                    "topicError",
                    "<color value=red>" +
                    " Please, fill the topic field and try again... " +
                    "<color value=black>"
                },
                {
                    "negDurationError",
                    "<color value=red>" +
                    " Give at least this project a positive number of days... " +
                    "<color value=black>"
                },
                {
                    "topicInput",
                    String.Join(App.Renderer.SplitChar, ""+ 
                        "Give an interesting title to the new project:",
                        $"┌{new string('─', 60 + 2)}┐",
                        "│ <input regex='[\\w0-9 ]' length=60> │",
                        $"└{new string('─', 60 + 2)}┘")
                },
                {
                    "dateInput",
                    String.Join(App.Renderer.SplitChar, ""+
                        " YYYY   MM   DD ",
                        "┌────┐ ┌──┐ ┌──┐",

                        "│<input regex='[0-9]' length=4>│/" +
                        "│<input regex='[0-9]' length=2>│/" +
                        "│<input regex='[0-9]' length=2>│",

                        "└────┘ └──┘ └──┘")
                },
                {
                    "hitEnterHint",
                    "<color value=white> " +
                    "Fill in the project's details and press Enter to continue... " +
                    "<color value=black>"
                },
            };
        }
    }
}