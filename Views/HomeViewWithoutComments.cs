using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class HomePageWithoutComments : ContentView
    {
        public HomePageWithoutComments(
            Dictionary<string, string> sharedResources,
            Dictionary<string, string> pageModifiers)
            :base(sharedResources)
        {
            SharedResources = sharedResources;
            PageModifiers = pageModifiers;

            Layout = new List<string>()
            {
                "topBar", 
                "[emptyLine]", 
                "[intro]",
                "3*[test]",
                "[emptyLine]", 
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "intro",
                    "Bienvenue à toi, $userName ! C'est cool walah <color value=blue>$qqch<color value=black>"
                },
                {
                    "test",
                    "--> | <input regex=[A-Za-z0-9]> | <--"
                },
            };
        }
    }
}