using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Views
{
    class HomePage : ContentView
    {
        public HomePage(Dictionary<string, string> userData)
        {
            SharedResources = App.Renderer.VisualResources;
            PageModifiers = userData;

            bool isAllowed = userData["userPrivilege"] != Privilege.Spectator.ToString();

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",
                
                "[intro]",
                "2*[emptyLine]",

                "[sortingMethodSelectors]",
                "[emptyLine]",

                isAllowed ? "[editMethodSelectors]" : "[emptyLine]",
                "[emptyLine]",

                "[quitOption]",

                "3*[emptyLine]",
                "botBar",
                "selectorHint"

             };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "intro",
                    "Welcome to Project Manager, select one of the following options below:"
                },
                {
                    "sortingMethodSelectors",
                    String.Join(App.Renderer.SplitChar, "",
                        "Sort & display projects",
                        "┌──────────────────────────┐",
                        "│ <selector value=0 text=' - Sort by Students '>     │",
                        "│ <selector value=1 text=' - Sort by Teachers '>     │",
                        "│ <selector value=2 text=' - Sort by Externs '>      │",
                        "│ <selector value=3 text=' - Sort by Courses '>      │",
                        "│ <selector value=4 text=' - Sort by School Years '> │",
                        "│ <selector value=5 text=' - Sort by Promotions '>   │",
                        "└──────────────────────────┘",
                        "┌──────────────────────────┐",
                        "│ <selector value=6 text=' - Sort by Date ASC ' color=blue>     │",
                        "│ <selector value=7 text=' - Sort by Date DESC ' color=blue>    │",
                        "│ <selector value=8 text=' - Sort by Keywords ' color=blue>     │",
                        "└──────────────────────────┘")
                },
                {
                    "editMethodSelectors",
                    String.Join(App.Renderer.SplitChar, "", 
                        "Add & remove projects",
                        "┌──────────────────────────┐",
                        "│ <selector value=9 text=' - Add project ' color=yellow>          │",
                        "│ <selector value=10 text=' - Remove project ' color=yellow>       │",
                        "└──────────────────────────┘")
                },
                {
                    "quitOption",
                    String.Join(App.Renderer.SplitChar, "",
                        "<selector value=11 text=' -- Exit -- ' color=red>")
                }
            };
        }
    }
}