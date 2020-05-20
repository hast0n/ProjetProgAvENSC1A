using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;
using ProjetProgAvENSC1A.Renderer;

namespace ProjetProgAvENSC1A.Views
{
    class EntryListView : ContentView
    {
        public EntryListView(Dictionary<string, string> entries, string entryType)
        {
            SharedResources = App.Renderer.VisualResources;

            Layout = new List<string>()
            {
                "2*emptyLine",
                "appName",
                "2*emptyLine",
                "userStatus",

                "topBar",
                "3*[emptyLine]",

                "[entryHint]",

                "[emptyLine]",

                "[entryList]",

                "3*[emptyLine]",
                "botBar",
                "selectorHint"
            };

            LocalResources = new Dictionary<string, string>()
            {
                {
                    "entryList",
                    GetFormattedData(entries)
                },
                {
                    "entryHint",
                    $"Select one of the following {entryType}s to display projects:"
                }
            };
        }

        public string GetFormattedData(Dictionary<string, string> entries)
        {
            StringBuilder sb = new StringBuilder();
            int maxLength = entries.Values.Max(e => e.Length);

            foreach (var kvp in entries)
            {
                sb.AppendFormat("<selector value={0} text=' {1} '>{2}",
                    kvp.Key, kvp.Value.PadRight(maxLength), App.Renderer.SplitChar);
            }

            return sb.ToString();
        }
    }
}