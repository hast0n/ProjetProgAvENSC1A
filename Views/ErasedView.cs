using ProjetProgAvENSC1A.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetProgAvENSC1A.Views
{
    class ErasedView : ContentView
    {
        public ErasedView(string name)
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

                "[erasedHint]",

                "4*[emptyLine]",
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "erasedHint",
                     $"{name} has been deleted from the database"
                },
            };
        }
    }
}
