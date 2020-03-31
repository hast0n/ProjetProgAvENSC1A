using System;
using CliLayoutRenderTools;
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

            LoginPage view = new LoginPage(console.VisualResources, console.SplitChar);

            console.Render(view);
        }
    }
}

// TODO: Lock edition to active field
// TODO: Press Return to edit next field
// TODO: Empty active field & backpace to access previous one