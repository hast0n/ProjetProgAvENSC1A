using System;
using System.Collections.Generic;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Controllers;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Views;

namespace ProjetProgAvENSC1A
{

    //TODO: Add following behaviour :
    // --> Instantiate renderer
    // --> Instantiate controllers
    // --> compile them in a queue
    // --> start render queue

    class App
    {
        static void Main(string[] args)
        {
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

            
            User user = LoginController.Authenticate();





            SelectorTestView selectorTestView = new SelectorTestView(console.VisualResources, console.SplitChar);

            console.Render(selectorTestView);
        }
    }
}

// TODO: Lock edition to active field
// TODO: Press Return to edit next field
// TODO:    --> add "completed" attribute to modifierDictionary for inputs
// TODO: Empty active field & backspace to access previous one