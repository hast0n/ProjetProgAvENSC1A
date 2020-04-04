using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class SelectorTestView : ContentPage
    {
        public SelectorTestView(
            Dictionary<string, string> sharedResources,
            char splitChar) : base(sharedResources)
        {
            Layout = new List<string>()
            {

                "3*emptyLine",
                "appName",
                "2*emptyLine",

                "topBar",
                "2*[emptyLine]",

                "[area1]", 
                "[area2]",
                "[wahoo]",
                "[wahoo2]",

                "2*[emptyLine]",
                "botBar",

                "emptyLine",
                "selectorHint"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "area1",
                    "<selector value=0 text='First choice'>"
                },
                {
                    "area2",
                    "<selector value=1 text='Second choice' color=blue>"
                },
                {
                    "wahoo",
                    "<selector value=2 text='Waaaahooo too many choices !!' color=red>"
                },
                {
                    "wahoo2",
                    "<selector value=3 text='No way this works that well' color=green>"
                }
            };
        }
    }
}
