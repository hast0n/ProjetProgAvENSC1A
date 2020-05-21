﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views
{
    class ProjectListView : ContentView
    {
        public ProjectListView(Dictionary<string, string> entries)
        {
            SharedResources = App.Renderer.VisualResources;

            bool existingProjects = (entries.Count()!=0);

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",

                existingProjects ? "[personHintTrue]" : "[personHintFalse]",
                "[emptyLine]",
                existingProjects ? "[entryList]" : "[emptyLine]",

                "3*[emptyLine]",
                "botBar",
                existingProjects ? "selectorHint" : "pressAnyHint"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "entryList",
                    GetFormattedData(entries)
                },
                {
                    "personHintTrue",
                    $"Select one of the following projects for more informations"
                }
                ,
                {
                    "personHintFalse",
                    $"There are no associated projects"
                }
            };
        }

        public string GetFormattedData(Dictionary<string, string> entries)
        {
            StringBuilder sb = new StringBuilder();

            if (entries.Count() != 0) 
            { 
                int maxLength = entries.Values.Max(e => e.Length);

                foreach (var kvp in entries)
                {
                    sb.AppendFormat("<selector value={0} text=` {1} `>{2}",
                        kvp.Key, kvp.Value.PadRight(maxLength), App.Renderer.SplitChar);
                }

                return sb.ToString();
            }
            else
            {
                return sb.ToString();
            }
        }
    }
}
