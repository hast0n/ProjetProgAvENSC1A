using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class HomePageWithoutComments : ContentPage
    {
        public HomePageWithoutComments(
            Dictionary<string, string> sharedResources,
            Dictionary<string, string> pageModifiers)
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
                    "Bienvenue à toi, $userName ! C'est cool walah <color:blue>$qqch<color:black>"
                },
                {
                    "test",
                    "--> | <input:[A-Za-z0-9]*> | <--"
                },
            };
        }
    }
}