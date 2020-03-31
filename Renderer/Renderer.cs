using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using ProjetProgAvENSC1A.Services;
using static System.String;


 //TODO: Change input regex and accept attributes
 //TODO:       --> placeholder
 //TODO:       --> length (max char input number)

 //TODO: Find a way to increase refresh performance 
 //TODO:       --> use backspace (mostly not possible)

 //TODO: Colors on input sections

 //TODO: Restrict regex char acceptance
 
 //TODO: Colors attributes foreground & background


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
                @"<(input|color|selector)[^\n\r\b<>]+>{1}"
            },
            {
                "color",
                @"<color (?:value=([a-zA-Z]+)){1}>{1}"
            },
            {
                // Will not trigger if :
                //      -> presence of unnecessary whitespaces
                //      -> length attr comes before regex attr
                "input",
                @"<input(?: regex=([^\s]+)){1}(?: length=([0-9]{1,2}))?>{1}"
            },
            {
                "selector",
                ""
            }
        }; 

        // Default placeholder for input
        public string DefaultInputPlaceholder;

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
            DefaultInputPlaceholder = " ";

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
        


        public Dictionary<int, Dictionary<string ,string>> Render(ContentPage page)
        {
            SetWindowSize();
            SetConsoleColorScheme("black");

            var userInputs = LaunchAndWaitForInput(page);

            return userInputs;
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
            string trimmedLine = line.Trim();
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
        


        private Dictionary<int, Dictionary<string, string>> LaunchAndWaitForInput(ContentPage page)
        {
            StringBuilder pageString = DumpScreen(page, out var modifierDictionary);
            
            int modIndex = modifierDictionary.GetModifierIndex();

            RenderScreen(pageString, modifierDictionary);
            
            if (modIndex != 0)
            {
                // Iterate over modifiers and set them
                while (modifierDictionary.Count > 0)
                {
                    // Prevent triggering for b, r or n in \b, \r or \n
                    string currentRegexPattern = $"^{modifierDictionary[modIndex][Constants.REGEX]}$";
                    string input = Input;

                    while (!Regex.IsMatch(input, currentRegexPattern))
                    {
                        Console.Write(Constants.BACKSPACE);

                        if (input.Equals(Constants.BACKSPACE) && modifierDictionary.HasSetModifiers())
                        {
                            modifierDictionary.WipeLastInput();
                            break;
                        }

                        if (input.Equals(Constants.CARRIAGE_RETURN) && modifierDictionary.GetFirstUnsetInput() == 0)
                        {
                            return (Dictionary<int, Dictionary<string, string>>) 
                                modifierDictionary.Where(kvp => 
                                    kvp.Value[Constants.TYPE].Equals(Constants.INPUT));
                        }

                        input = Input;
                    }

                    if (!input.Equals(Constants.BACKSPACE))
                    {
                        int length = int.Parse(modifierDictionary[modIndex][Constants.LENGTH]);
                        string userInput = modifierDictionary[modIndex][Constants.VALUE];
                        string res = $"{userInput}{input}";

                        modifierDictionary[modIndex][Constants.VALUE] = $"{userInput}{input}"
                            [.. (res.Length > length ? ^1 : ^0)];
                    }

                    RenderScreen(pageString, modifierDictionary);

                    modIndex = modifierDictionary.GetModifierIndex();
                }
            }

            ResetConsoleColors();
            return modifierDictionary;
        }

        private StringBuilder DumpScreen(ContentPage page,
            out Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Read screen to extract modifier indexes

            modifierDictionary = new Dictionary<int, Dictionary<string, string>>();

            StringBuilder output = new StringBuilder();

            var resources = GetViewContent(page);

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
                            break;
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

        private void RenderScreen(StringBuilder screenBuilder,
            Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Takes care of displaying colors and inputs values

            Console.Clear();
            // Set the index to which the frame has been rendered
            int renderedPartIndex = 0;

            foreach (var kvp in modifierDictionary
                .OrderBy(kvp => kvp.Key))
            {
                if (kvp.Value[Constants.TYPE].Equals(Constants.COLOR))
                {
                    // Entering color context

                    // Extract string before color modifier
                    string leftString = screenBuilder.ToString()
                        .Substring(renderedPartIndex,
                            kvp.Key - renderedPartIndex);
                    // Write it
                    Console.Write(leftString);
                    // Change console color
                    SetConsoleColorScheme(kvp.Value[Constants.VALUE]);
                    // Set rendering index
                    renderedPartIndex = kvp.Key;
                }
                else
                {
                    // Entering input context

                    char[] userInput = kvp.Value[Constants.VALUE].ToCharArray();
                    int currentLength = userInput.Length;
                    int maxLength = int.Parse(kvp.Value[Constants.LENGTH]);

                    for (int i = 0; i < maxLength; i++)
                    {
                        screenBuilder[kvp.Key + i] = i < currentLength
                            ? userInput[i]
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