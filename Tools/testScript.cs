using System;
using System.Collections.Generic;

namespace CliLayoutRenderTools
{
    class Test
    {
        static void test(string[] args)
        {
            var rd = new Renderer()
            {
                FrameWidth = 70
            };

            rd.UpdateResources();

            rd.AddVisualResources(new Dictionary<string, string>()
            {
                {
                    "intro",
                    "Bienvenue ! C'est cool walah <color:blue>yolooo<color:black>"
                },
                {
                    "test",
                    "--> | <input:[A-Za-z0-9]*> | <--"
                },
            });

            var screen = new List<string>()
            {
                "topBar", "[emptyLine]", "[intro]", "3*[test]", "[emptyLine]", "botBar"
            };

            rd.RenderAndWaitForInput(screen);

        }
    }
}