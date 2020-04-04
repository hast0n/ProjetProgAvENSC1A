using System;
using System.Collections.Generic;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A
{
    class App
    {
        static void Main(string[] args)
        {
            // Pour le tuto : utiliser la ligne suivante et commenter le rester
            // Les fichiers se trouvent dans .\Renderer\testScript.cs et .\Views\HomePage.cs
            //RenderTests.Test();

            Renderer console = new Renderer() { FrameWidth = 70 };
            
            console.SetDefaultResources();
            console.AddVisualResources(new Dictionary<string, string>() {
                {
                    "appName", 
                    "--- PROJECT MANAGER ---"
                },
                {
                    "userStatus", 
                    "$userStatus"
                }
            });

            //LoginPage logInView = new LoginPage(console.VisualResources, console.SplitChar);

            //var loggingInfoDictionary = console.Render(logInView);



            SelectorTestView selectorTestView = new SelectorTestView(console.VisualResources, console.SplitChar);

            console.Render(selectorTestView);
        }


    }
}

// TODO: Lock edition to active field
// TODO: Press Return to edit next field
// TODO:    --> add "completed" attribute to modifierDictionary for inputs
// TODO: Empty active field & backspace to access previous one