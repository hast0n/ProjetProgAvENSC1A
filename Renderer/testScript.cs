using System;
using System.Collections.Generic;
using ProjetProgAvENSC1A.Views;

namespace CliLayoutRenderTools
{
    class RenderTests
    {
        public static void Test()
        {
            var renderer = new Renderer()
            {
                FrameWidth = 70
            };

            renderer.SetDefaultResources();

            renderer.AddVisualResources(new Dictionary<string, string>()
            {
                {
                    "intro",
                    "Bienvenue ! C'est cool walah <color:blue>yolooo"
                },
                {
                    "test",
                    "--> | <input:[A-Za-z0-9]*> | <--"
                },
            });

            //var screen = new List<string>()
            //{
            //    "topBar", "[emptyLine]", "[intro]", "[emptyLine]", "botBar"
            //};
            
            renderer.RenderAndWaitForInput(new HomePage(renderer.VisualResources));
        }
    }
}