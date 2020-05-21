using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views.NewProjectViews
{
    class NewDeliverableView : ContentView
    {
        public NewDeliverableView()
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

                "[infoInput]",

                "3*[emptyLine]",
                "botBar",
                "selectorHint"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "infoInput",
                    String.Join(App.Renderer.SplitChar, ""+
                        "Give an interesting description to the deliverable:",
                        $"┌{new string('─', 90 + 2)}┐",
                        "│ <input regex=`[\\w0-9 ]` length=90> │",
                        $"└{new string('─', 90 + 2)}┘")
                },
            };
        }

    }
}
