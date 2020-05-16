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
        public CurrentOrAllView()
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
                    "<selector value=0 text=' - Currents Projects '> ",
                    "<selector value=1 text=' - All Projects'>")
                },
                {
                    "personHint",
                    $"Select one of the following choices to display projects"
                }
            };
        }
    }
}
