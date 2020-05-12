using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


// TODO : Surcharge de ToString() ?


namespace CliLayoutRenderTools
{
    public class ContentView
    {
        // Define default variable pattern regex
        public static readonly string VarPatternRegex = @"\$([a-zA-Z_\x80-\xff][a-zA-Z0-9_\x80-\xff]*)";
        // Define local visual resources
        public Dictionary<string, string> LocalResources { get; protected set; }
        // Get global visual resources
        protected Dictionary<string, string> SharedResources { get; set; }
        // Get page attributes values
        public Dictionary<string, string> PageModifiers { get; protected set; }
        // Define page layout
        public List<string> Layout { get; protected set; }
        // Define property for accessing serialized page
        public Dictionary<string, string> SerializedResources => Serialize();
        // Get all default uncomputed related visual resources
        public List<string> VisualResources => Layout.Select(
            selector: key => GetResourceOrDefault(key)[1]
        ).Distinct().ToList();
        // Get or set a layout element
        public string this[int index] => Layout[index];
        // Get or set a visual resource using resource identifier
        public string this[string identifier]
        {
            get => GetResourceOrDefault(identifier)[1];
            set => LocalResources[identifier] = value;
        }



        public ContentView(Dictionary<string, string> sharedResources = null)
        {
            SharedResources = sharedResources ?? new Dictionary<string, string>();
        }



        public void Clear() => LocalResources = new Dictionary<string, string>();

        public void AddResource(string key, string value) => LocalResources.Add(key, value);

        public void AddResource(Dictionary<string, string> extendDictionary)
        {
            foreach (var kvp in extendDictionary)
            {
                LocalResources.Add(kvp.Key, kvp.Value);
            }
        }
        


        private string[] GetResourceOrDefault(string resourceIdentifier)
        {
            string key = Renderer.GetResourceName(resourceIdentifier, out GroupCollection groups);
            string errorMessage = $"<color value=red>" +
                                  $"<--! Missing Resource for Identifier \'{resourceIdentifier}\' !-->" +
                                  $"<color value=black>";

            // will return value in LocalResources if exists in both
            return new string[]
            {
                key,
                LocalResources.GetValueOrDefault(key,
                    SharedResources.GetValueOrDefault(key,
                        errorMessage))
            };
        }

        private Dictionary<string, string> Serialize()
        {
            Regex r = new Regex(VarPatternRegex);
            var temp = new Dictionary<string, string>();

            foreach (var resourceIdentifier in Layout)
            {
                string[] resource = GetResourceOrDefault(resourceIdentifier);

                var matches = r.Matches(resource[1]);
                
                foreach (Match match in matches)
                {
                    var g = match.Groups;
                    try
                    {
                        resource[1] = resource[1].Replace(g[0].Value, PageModifiers[g[1].Value]);
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new AttributeValueNotFoundException(
                            $"Value not specified for attribute \'{g[1].Value}\'.");
                    }
                }

                if (!temp.ContainsKey(resource[0])) temp.Add(resource[0], resource[1]);
            }

            return temp;
        }
    }
}