using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;


 //TODO : Change input regex and accept attributes
 //TODO :       --> placeholder
 //TODO :       --> length (max char input number)

 //TODO : Find a way to increase refresh performance 
 //TODO :       --> use backspace (mostly not possible)

 //TODO : Colors on input sections

 //TODO : Restrict regex char acceptance


namespace CliLayoutRenderTools
{
    public class Renderer
    {
        #region Local Variables

        // visual elements to be displayed on screen
        public Dictionary<string, string> VisualResources;

        // console height
        public int WindowHeight;

        // game left padding
        public int FrameMargin;

        // game frame width
        public int FrameWidth;

        #region To find a way to remove

        // Default char for horizontal lines
        public char HorizontalLineChar;

        // Default char for vertical lines
        public char VerticalLineChar;

        #endregion

        // Default char to split strings on for carriage return
        public char SplitChar;

        // Regular Expression to detect if there are modifiers in a visual resource
        //public static string RegexTextAttributeDelimiterPattern = @"(<.*>)";

        // RegEx to detect if a visual resource needs to be framed or repeated (quite heavy...)
        private const string LayoutElementPattern = @"^(?:([1-9]+)\*)?(?:(?:\[([A-za-z0-9]+)\])||([A-Za-z0-9]+))$";

        // RegEx to get modifier index in visual resource
        //public static readonly string RegexInputDelimiterPattern = @"<(?:input|color):[^>]+>";

        // RegEx to extract modifier values in visual resource
        //public static readonly string RegexInputParamDelimiterPattern = @"<(input|color):([^>]+)>";
        
        // RegEx to detect/extract inputs & attributes
        public static readonly Dictionary<string, string> RegexPatterns = new Dictionary<string, string>()
        {
            {
                "any",
                @"<(input|color|selector).+>"
            },
            {
                "color",
                @"<color (?:value=([a-zA-Z]+)){1}>"
            },
            {
                // Will not trigger if :
                //      -> presence of unnecessary whitespaces
                //      -> length attr comes before regex attr
                "input",
                @"<input(?: regex=([^\s]+)){1}(?: length=([0-9]{1,2}))?>"
            },
            {
                "selector",
                ""
            }
        }; 

        // Default placeholder for input
        public string DefaultInputValue;

        // Boolean that asserts if inputs are being allowed or not
        //public bool CanInput;

        // Default console colors configuration
        private readonly ConsoleColor[] _defaultColors;

        // Console background colors
        public Dictionary<string, ConsoleColor> ConsoleBackgroundColors;

        // Console foreground colors
        public Dictionary<string, ConsoleColor> ConsoleTextColors;

        #endregion

        #region Local Properties
        public string PaddingString => new string(' ', FrameMargin);
        public string HorizontalBar => new string(HorizontalLineChar, FrameWidth - 2);
        public int WindowWidth => FrameWidth + FrameMargin* 2;
        
        private string Input => $"{Console.ReadKey().KeyChar}";
        #endregion

        public Renderer()
        {
            // https://regexr.com/4s4lb

            // Save console colors config
            _defaultColors = new ConsoleColor[]
            {
                Console.BackgroundColor,
                Console.ForegroundColor
            }; 

            //CanInput = true;
            FrameWidth = 100;
            WindowHeight = 50;
            FrameMargin = 5;
            HorizontalLineChar = '─';
            VerticalLineChar = '│';
            SplitChar = '\n';
            DefaultInputValue = " ";

            // Define basic console colors dictionary to easily access them
            ConsoleBackgroundColors = new Dictionary<string, ConsoleColor>()
            {
                {"red", ConsoleColor.Red},
                {"cyan", ConsoleColor.Cyan},
                {"blue", ConsoleColor.Blue},
                {"gray", ConsoleColor.Gray},
                {"green", ConsoleColor.Green},
                {"black", ConsoleColor.Black},
                {"white", ConsoleColor.White},
                {"yellow", ConsoleColor.Yellow},
                {"magenta", ConsoleColor.Magenta},
                {"darkred", ConsoleColor.DarkRed},
                {"darkblue", ConsoleColor.DarkBlue},
                {"darkcyan", ConsoleColor.DarkCyan},
                {"darkgray", ConsoleColor.DarkGray},
                {"darkgreen", ConsoleColor.DarkGreen},
                {"darkyellow", ConsoleColor.DarkYellow},
                {"darkmagenta", ConsoleColor. DarkMagenta},
            };

            // Define foreground color according to background color for better readability
            ConsoleTextColors = new Dictionary<string, ConsoleColor>()
            {
                {"red", ConsoleColor.White},
                {"cyan", ConsoleColor.Black},
                {"blue", ConsoleColor.White},
                {"gray", ConsoleColor.Black},
                {"green", ConsoleColor.Black},
                {"black", ConsoleColor.White},
                {"white", ConsoleColor.Black},
                {"yellow", ConsoleColor.Black},
                {"magenta", ConsoleColor.White},
                {"darkred", ConsoleColor.White},
                {"darkblue", ConsoleColor.White},
                {"darkcyan", ConsoleColor.White},
                {"darkgray", ConsoleColor.White},
                {"darkgreen", ConsoleColor.White},
                {"darkyellow", ConsoleColor.White},
                {"darkmagenta", ConsoleColor.White},
            };

            // Initialize default viewing experience
            SetConsoleColorScheme("black");
        }

        private void SetWindowSize()
        {
            if (Console.WindowWidth != WindowWidth) Console.WindowWidth = WindowWidth;
            if (Console.WindowHeight != WindowHeight) Console.WindowHeight = WindowHeight;
        }

        public void SetDefaultResources()
        {
            VisualResources = new Dictionary<string, string>()
            {
                {
                    "topBar",
                    $"{PaddingString}┌{HorizontalBar}┐"
                },
                {
                    "botBar",
                    $"{PaddingString}└{HorizontalBar}┘"
                },
                {
                    "emptyLine",
                    Empty
                },
            };
        }

        public void SetConsoleColorScheme(string color)
        {
            Console.BackgroundColor = ConsoleBackgroundColors[color];
            Console.ForegroundColor = ConsoleTextColors[color];
        }

        public void ResetConsoleColors()
        {
            Console.BackgroundColor = _defaultColors[0];
            Console.ForegroundColor = _defaultColors[1];
        }



        public void AddVisualResources(string key, string value)
        {
            VisualResources.Add(key, value);
        }
        
        public void AddVisualResources(Dictionary<string, string> extendDictionary)
        {
            foreach (var kvp in extendDictionary)
            {
                VisualResources.Add(kvp.Key, kvp.Value);
            }
        }
        


        public static string GetResourceName(string identifier, out GroupCollection groups)
        {
            groups = Regex.Match(identifier, LayoutElementPattern).Groups;
            var key = groups.Count > 1
                ? IsNullOrEmpty(groups[2].Value)
                    ? groups[3].Value
                    : groups[2].Value
                : identifier;
        
            return key;
        }

        public List<string> GetViewContent(ContentPage view)
        {
            var resources = view.SerializedResources;
            List<string> viewContent = new List<string>();
            
            foreach (var res in view.Layout)
            {
                string key = GetResourceName(res, out GroupCollection groups);
                string line = resources[key];
                viewContent.Add(GetComputedResource(res, line));
            }

            return viewContent;
        }


        public string GetComputedResource(string identifier, string baseResource, bool compute = true)
        {
            return GetResourceRepr(identifier, baseResource, compute);
        }

        public string GetComputedResource(string identifier, bool compute = true)
        {
            string baseResource = VisualResources.GetValueOrDefault(identifier)
                                   ?? $"<Resource '{identifier}' Not Found>";

            return GetResourceRepr(identifier, baseResource, compute);
        }

        private string GetResourceRepr(string identifier, string baseResource, bool compute)
        {
            string key = GetResourceName(identifier, out GroupCollection groups);

            if (compute)
            {
                var modLine = baseResource;
                var needsEncaps = !IsNullOrEmpty(groups[2].Value);
                var needsRep = !IsNullOrEmpty(groups[1].Value);

                if (needsEncaps) modLine = FrameMultipleLines(modLine);

                var buffer = Empty;

                if (needsRep)
                {
                    var repString = groups[1].Value;
                    int repN = int.Parse(repString);

                    modLine = Join('\n', Enumerable.Repeat(modLine, repN));
                }

                return modLine;
            }

            return baseResource;
        }



        private string FrameMultipleLines(string linesString)
        {
            // Frame multiple lines from one visual resource

            try
            {
                // extract lines with multiline delimiters
                string[] linesArray = linesString.Split(SplitChar)
                    .Where(l => !IsNullOrEmpty(l)).ToArray();

                if (linesArray.Length < 1)
                {
                    throw new StackOverflowException();
                }

                StringBuilder stringBuilder = new StringBuilder();

                // Append each framed line to StringBuilder and return it
                var collection = linesArray
                    .Where(l => !IsNullOrEmpty(l)).ToList();
                var lastIndex = collection.Count() - 1;

                for (int i = 0; i < collection.Count(); i++)
                {
                    string line = collection[i];
                    stringBuilder.AppendFormat("{0}{1}",
                        FrameOneLine(line),
                        i == lastIndex ? "" : "\n");
                }

                return stringBuilder.ToString();
            }
            catch (StackOverflowException)
            {
                // no multiline delimiter
                return FrameOneLine(linesString);
            }
        }

        private string FrameOneLine(string line)
        {
            // Frame one line
            return EncapsulateString(line).Pad(FrameMargin);
        }
        
        private string EncapsulateString(string line)
        {
            // Frame a line to match the game outside border

            Regex r = new Regex(RegexPatterns["any"]);
            string trimmedLine = line.Trim();
            string pseudoLine = trimmedLine;
            var match = r.Match(pseudoLine);

            while (match.Success)
            {
                string replacement = match.Groups[1].Value.Equals("input") ? " " : Empty;
                pseudoLine = r.Replace(pseudoLine, replacement, 1);
                match = r.Match(pseudoLine);
            }

            int lineLength = pseudoLine.Length;
            int width = FrameWidth;
            char symb = VerticalLineChar;
            int padding;

            // TODO: throw error if line is too long
            // the following behaviour is very foolish
            if (lineLength > width)
            {
                int excess = lineLength - width - 2; // -2 corresponds to the 2 border symbols
                trimmedLine = trimmedLine.Substring(excess / 2, width - 2);
                padding = 0;
            }
            else
            {
                padding = (width - lineLength) / 2 - 1;
            }

            bool colParity = lineLength.IsOdd() != width.IsOdd();

            string paddingString = new string(' ', padding);

            return Format("{2}{1}{0}{1}{3}{2}",
                trimmedLine, paddingString, symb, (colParity) ? " " : "");
        }
        


        public Dictionary<int, string[]> Render(ContentPage page)
        {
            SetWindowSize();
            SetConsoleColorScheme("black");

            var userInputs = LaunchAndWaitForInput(page);

            return userInputs;
        }



        private Dictionary<int, string[]> LaunchAndWaitForInput(ContentPage page)
        {
            Dictionary<int, string[]> modifierDictionary;

            #region Render Period Initialization
            // Set useful local methods
            bool HasSetModifiers()
            {
                // Return a bool indicating if user has already input data
                return modifierDictionary
                    .Where(x => x.Value[0] != "<color>")
                    .Count(x => !x.Value[0].Equals(DefaultInputValue)) > 0;
            }
            int FirstUnsetInput()
            {
                // Get index of first unset input modifier
                return modifierDictionary
                    .Where(x => x.Value[0] != "<color>")
                    .OrderBy(x => x.Key)
                    .FirstOrDefault(x => x.Value[0].Equals(DefaultInputValue)).Key;
            }
            int LastSetInput()
            {
                // Get index of Last set input modifier
                return modifierDictionary
                    .Where(x => x.Value[0] != "<color>")
                    .OrderBy(x => x.Key)
                    .LastOrDefault(x => !x.Value[0].Equals(DefaultInputValue)).Key;
            }
            void WipeLastInput()
            {
                // Remove last user input
                try
                {
                    modifierDictionary[LastSetInput()][0] = DefaultInputValue;
                }
                catch (KeyNotFoundException) { /* DO NOTHING */ }
            }
            #endregion


            StringBuilder pageString = DumpScreen(page, out modifierDictionary);
            int modIndex = FirstUnsetInput() == 0 ? LastSetInput() : FirstUnsetInput();
            
            if (modIndex == 0)
            {
                RenderScreen(pageString, modifierDictionary);
            }
            else
            {
                RenderScreen(pageString, modifierDictionary);

                // Iterate over modifiers and set them
                while (modifierDictionary.Count > 0) // && CanInput)
                {
                    modIndex = FirstUnsetInput() == 0 ? LastSetInput() : FirstUnsetInput();

                    string currentRegexPattern = $"^{modifierDictionary[modIndex][1]}$";
                    string input = Input;

                    while (!Regex.IsMatch(input, currentRegexPattern)) // && CanInput)
                    {
                        Console.Write("\b");

                        if (input.Equals("\b") && HasSetModifiers())
                        {
                            WipeLastInput();
                            break;
                        }

                        if (input.Equals("\r") && FirstUnsetInput() == 0)
                        {
                            foreach (var item in modifierDictionary.Where(kvp => kvp.Value[0].Equals("<color>")).ToList())
                            {
                                modifierDictionary.Remove(item.Key);
                            }
                            return modifierDictionary;
                        }

                        input = Input;
                    }

                    if (!input.Equals("\b")) // && CanInput)
                    {
                        modifierDictionary[modIndex][0] = input;
                    }

                    RenderScreen(pageString, modifierDictionary);
                }
            }

            ResetConsoleColors();
            return modifierDictionary;
        }

        private StringBuilder DumpScreen(ContentPage page, out Dictionary<int, string[]> modifierDictionary)
        {
            // Read screen to extract modifier indexes

            modifierDictionary = new Dictionary<int, string[]>();

            StringBuilder output = new StringBuilder();

            var resources = GetViewContent(page);

            foreach (string line in resources)
            {
                string computedLine = line;

                Regex r = new Regex(RegexPatterns["any"]);
                var match = r.Match(computedLine);

                while (match.Success)
                {
                    // TODO: change regex pattern according to previously determined field type
                    var group = Regex.Match(match.Value, RegexPatterns["input"]).Groups;
                    int modIndex = output.Length + match.Index;
                    string replacement = Empty;

                    if (group[1].Value.Equals("input"))
                    {
                        replacement = DefaultInputValue;
                        modifierDictionary[modIndex] = new[] { replacement, group[2].Value };
                    }
                    else if (group[1].Value.Equals("color"))
                    {
                        replacement = Empty;
                        modifierDictionary[modIndex] = new[] { "<color>", group[2].Value };
                    }

                    // seems to need instance of Regex to use occurence replacement quantifier...
                    computedLine = r.Replace(computedLine, replacement, 1);
                    // search again for any new match (new input)
                    match = r.Match(computedLine);
                }

                output.AppendLine(computedLine);
            }

            return output;
        }

        private void RenderScreen(StringBuilder screenBuilder, Dictionary<int, string[]> modifierDictionary)
        {
            // Takes care of displaying colors and inputs values

            Console.Clear();
            // Set the index to which the frame has been rendered
            int renderedPartIndex = 0;

            foreach (var kvp in modifierDictionary.OrderBy(x => x.Key))
            {
                if (kvp.Value[0].Equals("<color>"))
                {
                    // Entering color context

                    // Extract string before color modifier
                    string leftString = screenBuilder.ToString().Substring(renderedPartIndex, kvp.Key - renderedPartIndex);
                    // Write it
                    Console.Write(leftString);
                    // Change console color
                    SetConsoleColorScheme(kvp.Value[1]);
                    // Set rendering index
                    renderedPartIndex = kvp.Key;
                }
                else
                {
                    // Entering input context

                    // Put user input in line to display
                    screenBuilder[kvp.Key] = char.Parse(kvp.Value[0]);
                }
            }

            // Display remaining characters that contains no modifiers
            Console.WriteLine(screenBuilder.ToString().Substring(renderedPartIndex, screenBuilder.Length - renderedPartIndex));
        }
    }
}