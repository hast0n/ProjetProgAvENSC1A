using CliLayoutRenderTools;
using System.Collections.Generic;

namespace ProjetProgAvENSC1A.Views
{
    class SearchPage : ContentView
    {
        public SearchPage(Dictionary<string, string> contextData)
        {
            SharedResources = App.Renderer.VisualResources;
            PageModifiers = contextData;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",



                "3*[emptyLine]",
                "botBar",
                "selectorHint"

             };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "", ""
                }
            };
        }
    }
}