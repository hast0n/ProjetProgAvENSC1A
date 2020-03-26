using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    class HomePage : ContentPage
    {
        public HomePage(
            Dictionary<string, string> sharedResources,
            Dictionary<string, string> pageValues = null)
        {
            SharedResources = sharedResources;
            PageValues = pageValues;

            Layout = new List<string>()
            {
                "topBar", 
                "[emptyLine]", 
                "[intro]",
                "3*[test]",
                // "intro" et "test" sont spécifiques à HomePage, on le retrouve donc dans les LocalResources
                // Les autres ressources partégées doivent être passées en paramètre à la création de la page
                "[emptyLine]", 
                "botBar"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "intro",
                    "Bienvenue à toi, $userName ! C'est cool walah <color:blue>yolooo<color:black>"
                },
                {
                    "test",
                    "--> | <input:[A-Za-z0-9]*> | <--"
                },
            };

            this.Prepare();
        }
    }
}
