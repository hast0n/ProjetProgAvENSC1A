using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CliLayoutRenderTools
{
    public class ContentPage
    {
        // Define default variable pattern regex
        public static readonly string VarPatternRegex = @"\$([a-zA-Z_\x80-\xff][a-zA-Z0-9_\x80-\xff]*)";
        // Define local visual resources to be customized
        public Dictionary<string, string> LocalResources { get; protected set; }
        // Get global visual resources
        public Dictionary<string, string> SharedResources { get; protected set; }
        // Get page attributes values
        public Dictionary<string, string> PageValues { get; protected set; }
        // Define page layout
        public List<string> Layout { get; protected set; }
        // Define property for accessing serialized page
        public Dictionary<string, string> Serialized => Serialize();

        public void AddResource(string key, string value)
        {
            LocalResources.Add(key, value);
        }

        public void AddResource(Dictionary<string, string> extendDictionary)
        {
            foreach (var kvp in extendDictionary)
            {
                LocalResources.Add(kvp.Key, kvp.Value);
            }
        }

        public void Clear()
        {
            LocalResources = new Dictionary<string, string>();
        }

        protected void Prepare()
        {

        }

        private string GetResourceOrDefault(string resourceIdentifier)
        {
            string key = Renderer.GetResourceName(resourceIdentifier, out GroupCollection groups);
            string errorMessage = $"<--! Missing Resource for Identifier \"{resourceIdentifier}\" !-->";

            return LocalResources.GetValueOrDefault(key, 
                  SharedResources.GetValueOrDefault(key, errorMessage));
        }

        private Dictionary<string, string> Serialize()
        {
            Regex r = new Regex(VarPatternRegex);
            
            int i = 0;
            
            foreach (var resourceIdentifier in Layout)
            {
                string resource = GetResourceOrDefault(resourceIdentifier);

                var matches = r.Matches(resource);
                if (matches.Count > 1)
                {

                }

                i++;
            }

            return LocalResources;
        }
    }
}
