using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CliLayoutRenderTools
{
    public class ScreenTemplate : Renderer
    {
        public Dictionary<string, string> LocalResources = new Dictionary<string, string>();
        public List<string> Layout;
        public Regex VarPatternRegex;

        public ScreenTemplate(List<string> layout)
        {

            Layout = layout;
            VarPatternRegex = new Regex(RegexVariableIdentifierPattern);
        }

        public ScreenTemplate()
        {
            Layout = new List<string>();
            VarPatternRegex = new Regex(RegexVariableIdentifierPattern);
        }

        public void Clear()
        {
            LocalResources = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Serialized()
        {
            int i = 0;

            foreach (var resourceIdentifier in Layout)
            {
                string resource = VisualResources.GetValueOrDefault(
                    resourceIdentifier, "<--! " +
                        "Missing Resource for Identifier " +
                        $"\"{resourceIdentifier}\" " +
                        $"at index {i} " +
                        "!-->"
                    );

                var matches = VarPatternRegex.Matches(resource);
                if (matches.Count > 1)
                {

                }

                i++;
            }

            return LocalResources;
        }
    }

}
