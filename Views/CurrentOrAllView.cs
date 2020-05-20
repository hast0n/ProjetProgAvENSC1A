using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using System.Linq;


namespace ProjetProgAvENSC1A.Views
{
    class CurrentOrAllView : ContentView
    {
        public CurrentOrAllView(bool student)
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",

                "[personHint]",
                "[emptyLine]",
                "[entryList]",

                "3*[emptyLine]",
                "botBar",
                "selectorHint"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "entryList",
                    String.Join(App.Renderer.SplitChar, "",
                    "<selector value=0 text=' - All Projects '> ",
                    "<selector value=1 text=' - Currents Projects '>",
                    student ? "<selector value=2 text=' - Promotion's Currents Projects '>":"")
                },
                {
                    "personHint",
                    "Select one of the following choices to display projects:"
                }
            };
        }
    }
}
