using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Services;
using static System.String;


 //TODO: Colors on input sections

 //TODO: Restrict regex char acceptance
 
 //TODO: Colors attributes foreground & background

 //TODO: /!\ prevent & feedback over use of input and selector on same page /!\


namespace ProjetProgAvENSC1A.Renderer
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
        
        // RegEx to detect/extract fields & attributes
        public static readonly Dictionary<string, string> RegexPatterns = new Dictionary<string, string>()
        {
            {
                "any",
                @"<(input|color|selector)[^\t\n\r\b<>]+>{1}"
            },
            {
                "color",
                @"<color (?:value=([a-zA-Z]+)){1}>{1}"
            },
            {
                // Will not trigger if :
                //      -> presence of unnecessary whitespaces
                //      -> length attr comes before regex attr
                //      -> no regex attribute
                "input",
                @"<input (?:regex='([^\r\n\t\f\v]+)){1}'(?: length=([0-9]{1,2}))?(?: hidden=(true|false))?>{1}"
                //@"<input (?:regex='([^\r\n\t\f\v]+)){1}'(?: length=([0-9]{1,2}))?>{1}"
            },
            {
                // Every character is allowed in the text attribute
                // as long as they do not include \t, \n, \r, \b, <, >
                // as defined in the 'any' regex
                "selector",
                @"<selector (?:value=([0-9]{1,2})){1} (?:text='([^\t\n\r<>]+)'){1}" +
                    "(?: color=([a-zA-Z]+))?(?: (selected))?>"
            }
        }; 

        // Default placeholder for input
        public string DefaultInputPlaceholder;

        public readonly string[] inputBreakers;

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

        public string ConsoleColorTheme
        {
            get => ConsoleBackgroundColors.First(kvp 
                => kvp.Value == Console.BackgroundColor).Key;
            set
            {
                Console.BackgroundColor = ConsoleBackgroundColors[value];
                Console.ForegroundColor = ConsoleTextColors[value];
            }
        }

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
            DefaultInputPlaceholder = " ";
            inputBreakers = new string[] {Constants.BACKSPACE, Constants.CARRIAGE_RETURN};

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
            ConsoleColorTheme = "black";
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
                    $"┌{HorizontalBar}┐"
                },
                {
                    "botBar",
                    $"└{HorizontalBar}┘"
                },
                {
                    "emptyLine",
                    Empty
                },
                {
                    "selectorHint",
                    "<color value=white>" +
                    $" Highlight with {Constants.SELECTOR_BACKWARD.ToUpper()} and " +
                    $"{Constants.SELECTOR_FORWARD.ToUpper()} & press <Enter> to confirm... " +
                    "<color value=black>"
                },
                {
                    "inputFieldHint", 
                    ""
                },
                {
                    "pressAnyHint",
                    "<color value=white> Press any key to continue... <color value=black>"
                }
            };
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
        


        public ImmutableDictionary<int, Dictionary<string ,string>> Render(ContentView view)
        {
            SetWindowSize();
            ConsoleColorTheme ="black";

            var userInputs = LaunchAndWaitForInput(view);

            return userInputs;
        }

        public List<string> GetViewContent(ContentView view)
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

                modLine = FrameMultipleLines(modLine, needsEncaps);

                var buffer = Empty;

                if (needsRep)
                {
                    var repString = groups[1].Value;
                    int repN = int.Parse(repString);

                    modLine = Join(Constants.LINE_FEED, Enumerable.Repeat(modLine, repN));
                }

                return modLine;
            }

            return baseResource;
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



        private string FrameMultipleLines(string linesString, bool addSideChar = true)
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
                        FrameOneLine(line, addSideChar),
                        i == lastIndex ? Empty
                            : Constants.LINE_FEED);
                }

                return stringBuilder.ToString();
            }
            catch (StackOverflowException)
            {
                // no multiline delimiter
                return FrameOneLine(linesString, addSideChar);
            }
        }

        private string FrameOneLine(string line, bool addSideChar)
        {
            // Frame one line
            return EncapsulateString(line, addSideChar).Pad(FrameMargin, !addSideChar);
        }
        
        private string EncapsulateString(string line, bool addSideChar)
        {
            // Frame a line to match the game outside border

            Regex r = new Regex(RegexPatterns[Constants.ANY]);
            string trimmedLine = line.TrimEnd();
            string pseudoLine = trimmedLine;
            var match = r.Match(pseudoLine);

            while (match.Success)
            {
                string replacement;

                // The following behaviour does not belong here ¯\_(ツ)_/¯
                if (match.Groups[1].Value.Equals(Constants.INPUT))
                {
                    var groups = new Regex(RegexPatterns[Constants.INPUT])
                        .Match(pseudoLine).Groups;
                    string length = groups[2].Value != Empty
                        ? groups[2].Value
                        : "1";

                    replacement = new string(
                        char.Parse(DefaultInputPlaceholder),
                        int.Parse(length));
                }
                else if (match.Groups[1].Value.Equals(Constants.SELECTOR))
                {
                    var groups = new Regex(RegexPatterns[Constants.SELECTOR])
                        .Match(pseudoLine).Groups;

                    replacement = groups[2].Value;
                }
                else
                {
                    replacement = match.Groups[1].Value.Equals(Constants.INPUT)
                        ? DefaultInputPlaceholder
                        : Empty;
                }

                pseudoLine = r.Replace(pseudoLine, replacement, 1);
                match = r.Match(pseudoLine);
            }

            int lineLength = pseudoLine.Length;
            int width = FrameWidth;
            char symb = VerticalLineChar;
            int padding;

            // TODO: throw error if line is too long
            // the following behaviour is very foolish (maybe not that much...)
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

            string paddingString = padding >= 0 ? new string(' ', padding) : "" ;

            return Format("{2}{1}{0}{1}{3}{2}",
                trimmedLine, paddingString, addSideChar ? symb : '\0', (colParity) ? " " : "");
        }



        private ImmutableDictionary<int, Dictionary<string, string>> LaunchAndWaitForInput(ContentView view)
        {
            StringBuilder pageString = DumpScreen(view, out var modifierDictionary);
            
            var res = new Dictionary<int, Dictionary<string, string>>()
                {{ 0, new Dictionary<string, string>() {{ Constants.TYPE, null }} }}
                .ToImmutableDictionary();
            
            if (modifierDictionary.ContainsField(Constants.INPUT))
            {
                int inputIndex = modifierDictionary.GetInputFieldIndex();
                
                HandleInput(modifierDictionary, inputIndex, pageString);

                var kvpEnumerable = modifierDictionary.Where(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.INPUT));
                
                modifierDictionary.TrimInputFields();

                res = kvpEnumerable.ToImmutableDictionary();
            }
            else if (modifierDictionary.ContainsField(Constants.SELECTOR))
            {
                modifierDictionary.RemoveSelectedDuplicate();

                HandleSelector(modifierDictionary, pageString);

                var kvpEnumerable = modifierDictionary.Where(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR));

                res = kvpEnumerable.ToImmutableDictionary();
            }
            else
            {
                RenderScreen(pageString, modifierDictionary);
                var _ = Input;
            }

            ResetConsoleColors();
            
            return res;
        }

        private void HandleSelector(
            Dictionary<int, Dictionary<string, string>> modifierDictionary, 
            StringBuilder pageString)
        {
            RenderScreen(pageString, modifierDictionary);

            int index = modifierDictionary.GetSelectedIndex();
            int prevIndex = index;

            string input = Input.ToLower();

            while (!input.Equals(Constants.CARRIAGE_RETURN))
            {
                modifierDictionary[index][Constants.SELECTED] = bool.FalseString;
                
                if (input.Equals(Constants.SELECTOR_BACKWARD))
                {
                    index = modifierDictionary.GetPreviousSelectorIndex(index);
                }
                else if (input.Equals(Constants.SELECTOR_FORWARD))
                {
                    index = modifierDictionary.GetNextSelectorIndex(index);
                }

                index = index != 0 ? index : prevIndex;

                modifierDictionary[index][Constants.SELECTED] = bool.TrueString;
                
                if (!index.Equals(prevIndex)) RenderScreen(pageString, modifierDictionary);

                Console.Write(Constants.BACKSPACE);

                input = Input.ToLower();

                prevIndex = index;
            }
        }

        private void HandleInput(
            Dictionary<int, Dictionary<string, string>> modifierDictionary,
            int inputIndex, StringBuilder pageString)
        {
            RenderScreen(pageString, modifierDictionary);

            while (modifierDictionary.Count > 0)
            {
                // Prevent triggering for b, r ,n in \b, \r or \n
                string currentRegexPattern = $"^{modifierDictionary[inputIndex][Constants.REGEX]}$";
                bool isHiddenInput = modifierDictionary[inputIndex][Constants.HIDDEN] == bool.TrueString;
                string input = Input;
                
                while (!Regex.IsMatch(input, currentRegexPattern))
                {
                    Console.Write(Constants.BACKSPACE);

                    if (input.Equals(Constants.BACKSPACE) && modifierDictionary.HasSetModifiers())
                    {
                        modifierDictionary.WipeLastInput();
                        break;
                    }

                    if (input.Equals(Constants.CARRIAGE_RETURN))
                    {
                        int index = modifierDictionary.GetFirstUnsetInput();

                        if (index != 0) 
                        {
                            string v = modifierDictionary[index][Constants.VALUE];
                            int l = int.Parse(modifierDictionary[index][Constants.LENGTH]);

                            modifierDictionary[index][Constants.VALUE] = v.PadRight(l);
                        }

                        if (modifierDictionary.IsReadyForReturn())
                        {
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }

                    input = Input;
                }

                if (!inputBreakers.Contains(input))
                {
                    int length = int.Parse(modifierDictionary[inputIndex][Constants.LENGTH]);
                    string userInput = modifierDictionary[inputIndex][Constants.VALUE];
                    string res = $"{userInput}{input}";

                    modifierDictionary[inputIndex][Constants.VALUE] = res[..(res.Length > length ? ^1 : ^0)];
                }

                RenderScreen(pageString, modifierDictionary);

                inputIndex = modifierDictionary.GetInputFieldIndex();
            }
        }



        private StringBuilder DumpScreen(ContentView view,
            out Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Read screen to extract modifier indexes

            modifierDictionary = new Dictionary<int, Dictionary<string, string>>();

            StringBuilder output = new StringBuilder();

            var resources = GetViewContent(view);

            foreach (string line in resources)
            {
                string computedLine = line;

                Regex r = new Regex(RegexPatterns[Constants.ANY]);
                var match = r.Match(computedLine);

                while (match.Success)
                {
                    // TODO: change regex pattern according to previously determined field type
                    string fieldType = match.Groups[1].Value;

                    var group = Regex.Match(match.Value, RegexPatterns[fieldType]).Groups;
                    int modIndex = output.Length + match.Index;
                    string replacement = Empty;

                    switch (fieldType)
                    {
                        case Constants.INPUT:

                            string length = group[2].Value != Empty
                                ? group[2].Value 
                                : "1";

                            replacement = new string(
                                char.Parse(DefaultInputPlaceholder), 
                                int.Parse(length));
                            bool.Parse("true");
                            modifierDictionary[modIndex] = new Dictionary<string, string>()
                            {
                                {
                                    Constants.TYPE,
                                    fieldType
                                },
                                {
                                    Constants.REGEX,
                                    group[1].Value
                                },
                                {
                                    Constants.REPLACEMENT,
                                    replacement
                                },
                                {
                                    Constants.LENGTH,
                                    length
                                },
                                {
                                    // Placeholder for user input
                                    Constants.VALUE,
                                    Empty
                                },
                                {
                                    Constants.HIDDEN,
                                    group[3].Value.Equals(string.Empty)
                                        ? bool.FalseString
                                        : bool.Parse(group[3].Value).ToString()
                                }
                            };

                            break;

                        case Constants.COLOR:

                            replacement = Empty;

                            modifierDictionary[modIndex] = new Dictionary<string, string>()
                            {
                                {
                                    Constants.TYPE,
                                    fieldType
                                },
                                {
                                    Constants.REPLACEMENT,
                                    replacement
                                },
                                {
                                    Constants.VALUE,
                                    group[1].Value
                                }
                            };

                            break;

                        case Constants.SELECTOR:

                            replacement = group[2].Value;

                            modifierDictionary[modIndex] = new Dictionary<string, string>()
                            {
                                {
                                    Constants.TYPE,
                                    Constants.SELECTOR
                                },
                                {
                                    Constants.INDEX,
                                    group[1].Value
                                },
                                {
                                    Constants.TEXT,
                                    group[2].Value
                                },
                                {
                                    Constants.COLOR,
                                    group[3].Value == Empty
                                        ? "white"
                                        : group[3].Value
                                },
                                {
                                    Constants.SELECTED, 
                                    group[4].Value == Constants.SELECTED
                                        ? Boolean.TrueString
                                        : Boolean.FalseString
                                }
                            };

                            break;
                    }

                    computedLine = r.Replace(computedLine, replacement, 1);
                    
                    // search again for any new match (new input)
                    match = r.Match(computedLine);
                }

                output.AppendLine(computedLine);
            }

            return output;
        }

        /// <summary>
        /// Refresh current view with given data
        /// </summary>
        /// <param name="screenBuilder"></param>
        /// <param name="modifierDictionary"></param>
        private void RenderScreen(StringBuilder screenBuilder,
            Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {

            Console.Clear();
            // Set the index to which the frame has been rendered
            int renderedPartIndex = 0;
            
            foreach (var kvp in modifierDictionary
                .OrderBy(kvp => kvp.Key))
            {
                string type = kvp.Value[Constants.TYPE];

                if (type.Equals(Constants.SELECTOR))
                {
                    // Entering selector context

                    // Extract string before color modifier
                    string leftString = screenBuilder.ToString()
                        .Substring(renderedPartIndex,
                            kvp.Key - renderedPartIndex);
                    // Write it
                    Console.Write(leftString);

                    bool isSelected = kvp.Value[Constants.SELECTED].Equals(bool.TrueString);

                    // Save current color theme
                    string currentColorTheme = ConsoleColorTheme;

                    if (isSelected)
                    {
                        // Change console color
                        ConsoleColorTheme = kvp.Value[Constants.COLOR];
                    }
                    
                    // Write selector text
                    string text = kvp.Value[Constants.TEXT];
                    Console.Write(text);

                    // Not really necessary here...
                    if (isSelected)
                    {
                        // Rollback color theme previous
                        ConsoleColorTheme = currentColorTheme;
                    }

                    // Override rendering index 
                    renderedPartIndex = kvp.Key + text.Length;
                }
                else if (type.Equals(Constants.COLOR))
                {
                    // Entering color context

                    // Extract string before color modifier
                    string leftString = screenBuilder.ToString()
                        .Substring(renderedPartIndex,
                            kvp.Key - renderedPartIndex);
                    // Write it
                    Console.Write(leftString);
                    // Change console color
                    ConsoleColorTheme = kvp.Value[Constants.VALUE];
                    // Set rendering index
                    renderedPartIndex = kvp.Key;
                }
                else
                {
                    // Entering input context
                    bool isHiddenField = bool.Parse(kvp.Value[Constants.HIDDEN]);

                    char[] userInput = kvp.Value[Constants.VALUE].ToCharArray();
                    int currentLength = userInput.Length;
                    int maxLength = int.Parse(kvp.Value[Constants.LENGTH]);

                    for (int i = 0; i < maxLength; i++)
                    {
                        screenBuilder[kvp.Key + i] = i < currentLength
                            ? isHiddenField
                                ? Constants.HIDDEN_CHAR 
                                : userInput[i]
                            : kvp.Value[Constants.REPLACEMENT][i];
                    }
                }
            }

            // Display remaining characters that contains no modifiers
            Console.WriteLine(screenBuilder.ToString()
                .Substring(renderedPartIndex,
                    screenBuilder.Length - renderedPartIndex));
        }
    }
}