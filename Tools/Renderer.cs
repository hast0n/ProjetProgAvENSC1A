using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;


// TODO : Add variable insertion and compilation within string 


namespace CliLayoutRenderTools
{
    public class Renderer
    {
        #region Local Variables
        // visual elements to be displayed on screen
        public Dictionary<string, string> VisualResources;
        // console width
        public int WindowWidth;
        // console height
        public int WindowHeight;
        // game left padding
        public int FrameMargin;
        // game frame width
        public int FrameWidth;
        // Default char for horizontal lines
        public char HorizontalLineChar;
        // Default char for vertical lines
        public char VerticalLineChar;
        // Default char to use to split strings for carriage return
        public char SplitChar;
        // Default white space line for left padding
        public string PaddingString;
        // Default empty line
        public string EmptyLine;
        // Regular Expression to detect if there are modifiers in a visual resource
        public string RegexTextAttributeDelimiterPattern;
        // RegEx to detect if a visual resource needs to be framed or repeated
        public string RegexScreenParamDelimiterPattern;
        // RegEx to get modifier index in visual resource
        public string RegexInputDelimiterPattern;
        // RegEx to extract modifier values in visual resource
        public string RegexInputParamDelimiterPattern;
        // RegEx to extract variables names in visual resource
        public string RegexVariableIdentifierPattern;
        // Default Horizontal bar
        public string HorizontalBar;
        // Default placeholder for input
        public string DefaultInputValue;
        // Boolean that asserts if inputs are being allowed or not
        public bool CanInput;
        #endregion

        // Console background colors
        public Dictionary<string, ConsoleColor> ConsoleBackgroundColors;
        // Console foreground colors
        public Dictionary<string, ConsoleColor> ConsoleTextColors;

        public Renderer()
        {
            CanInput = true;
            FrameWidth = 100;
            WindowHeight = 50;
            FrameMargin = 5;
            HorizontalLineChar = '─';
            VerticalLineChar = '│';
            SplitChar = '\n';
            DefaultInputValue ??= " ";
            // https://regexr.com/4s4lb
            RegexVariableIdentifierPattern = @"\$([a-zA-Z_\x80-\xff][a-zA-Z0-9_\x80-\xff]*)";
            RegexTextAttributeDelimiterPattern = @"(<.*>)";
            RegexScreenParamDelimiterPattern = @"^(?:([1-9]+)\*)?(?:(?:\[([A-za-z0-9]+)\])||([A-Za-z0-9]+))$";
            RegexInputDelimiterPattern = @"<(?:input|color):[^>]+>";
            RegexInputParamDelimiterPattern = @"<(input|color):([^>]+)>";


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

            UpdateResources();

            ChangeCliColorScheme("white");
        }
        public void UpdateResources()
        {
            // Set default values according to instance parameters
            //FrameWidth = WindowWidth - FramePadding * 2 - 4;
            PaddingString = new string(' ', FrameMargin);
            HorizontalBar = new string(HorizontalLineChar, FrameWidth - 2);

            WindowWidth = FrameWidth + FrameMargin * 2;

            PaddingString = new string(' ', FrameMargin);
            HorizontalBar = new string(HorizontalLineChar, FrameWidth - 2);

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

            Console.SetWindowSize(WindowWidth, WindowHeight);
        }

        public void ChangeCliColorScheme(string color)
        {
            Console.BackgroundColor = ConsoleBackgroundColors[color];
            Console.ForegroundColor = ConsoleTextColors[color];
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

        public string GetResourceRepr(string keyIdentifier, bool compute = true)
        {
            var groups= Regex.Match(keyIdentifier, RegexScreenParamDelimiterPattern).Groups;
            var key = groups.Count > 1 
                ? IsNullOrEmpty(groups[2].Value)
                    ? groups[3].Value 
                    : groups[2].Value
                : keyIdentifier;

            var baseResource = VisualResources.GetValueOrDefault(key);
            baseResource ??= $"<Resource '{key}' Not Found>";

            if (compute)
            {
                var modLine = baseResource;
                var needsEncaps = !IsNullOrEmpty(groups[2].Value);
                var needsRep = !IsNullOrEmpty(groups[1].Value);

                if (needsEncaps) modLine = FrameOneLine(modLine);

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

        public string EncapsulateString(string line)
        {
            // Frame a line to match the game outside border

            Regex r = new Regex(RegexInputParamDelimiterPattern);
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

        public string FrameOneLine(string line)
        {
            // Frame one line
            return EncapsulateString(line).Pad(FrameMargin);
        }

        public string FrameMultipleLines(string linesString)
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
                foreach (string line in linesArray.Where(l => !IsNullOrEmpty(l)))
                {
                    stringBuilder.Append(FrameOneLine(line));
                }

                return stringBuilder.ToString();
            }
            catch (StackOverflowException)
            {
                // no multiline delimiter
                return FrameOneLine(linesString);
            }
        }

        public Dictionary<int, string[]> RenderAndWaitForInput(
            List<string> screen, Dictionary<string, object> variables = null)
        {
            #region Render Period Initialization
            Dictionary<int, string[]> modifierDictionary = new Dictionary<int, string[]>();

            // Set useful local methods
            bool HasSetModifiers()
            {
                // Return a bool indicating if user has already input data
                return modifierDictionary.Where(x => x.Value[0] != "<color>")
                           .Count(x => !x.Value[0].Equals(DefaultInputValue)) > 0;
            }
            int FirstUnsetInput()
            {
                // Get index of first unset input modifier
                return modifierDictionary.Where(x => x.Value[0] != "<color>").OrderBy(x => x.Key)
                    .FirstOrDefault(x => x.Value[0].Equals(DefaultInputValue)).Key;
            }
            int LastSetInput()
            {
                // Get index of Last set input modifier
                return modifierDictionary.OrderBy(x => x.Key).Where(x => x.Value[0] != "<color>")
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

            StringBuilder screenString = DumpScreen(screen, ref modifierDictionary);
            RenderScreen(screenString, modifierDictionary);

            // Get the index of current modifier to set
            int modIndex = (FirstUnsetInput() == 0) ? LastSetInput() : FirstUnsetInput();

            if (modIndex == 0)
            {
                // no input modifiers in screen
                RenderScreen(screenString, modifierDictionary);
                //var input = Console.ReadKey();
            }
            else
            {
                // Iterate over modifiers and set them

                while (modifierDictionary.Count > 0 && CanInput)
                {
                    modIndex = (FirstUnsetInput() == 0) ? LastSetInput() : FirstUnsetInput();

                    string currentRegexPattern = modifierDictionary[modIndex][1];
                    string input = $"{Console.ReadKey().KeyChar}";

                    while (!Regex.IsMatch(input, currentRegexPattern) && CanInput)
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

                        input = $"{Console.ReadKey().KeyChar}";
                    }

                    if (!input.Equals("\b") && CanInput)
                    {
                        modifierDictionary[modIndex][0] = input;
                    }

                    RenderScreen(screenString, modifierDictionary);
                }
            }

            return modifierDictionary;
        }

        private StringBuilder DumpScreen(List<string> screen, ref Dictionary<int, string[]> modifierDictionary)
        {
            // Read screen to display and extract modifier indexes

            StringBuilder output = new StringBuilder();

            foreach (string lineIdentifier in screen)
            {
                string line = GetResourceRepr(lineIdentifier);

                Regex r = new Regex(RegexInputDelimiterPattern);
                var match = r.Match(line);

                while (match.Success)
                {
                    var group = Regex.Match(match.Value, RegexInputParamDelimiterPattern).Groups;
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
                    line = r.Replace(line, replacement, 1);
                    // search again for any new match (new input)
                    match = r.Match(line);
                }

                output.AppendLine(line);
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
                    ChangeCliColorScheme(kvp.Value[1]);
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
