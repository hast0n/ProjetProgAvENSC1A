using ProjetProgAvENSC1A.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class SearchView : ContentView
    {
        public SearchView()
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "4*[emptyLine]",

                "[keyInput]",

                "4*[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "keyInput",
                    String.Join(App.Renderer.SplitChar, ""+
                        "                   ┌──────────────────────┐",
                        "Enter Keyword :    │ <input regex='[\\w0-9 ]' length=20> │",
                        "                   └──────────────────────┘")
                },
            };
        }
    }
}
