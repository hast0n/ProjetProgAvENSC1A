using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProjetProgAvENSC1A.Services;

namespace CliLayoutRenderTools
{
    // A simple class used to help the Renderer to frame content
    static class RendererExtensions
    {
        public static string Pad(this string line, int padding, bool removeOne)
        {
            return $"{new string(' ', padding - (removeOne ? 1 : 0))}{line}";
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }



        public static bool HasSetModifiers(this Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Return a bool indicating if user has already input data
            return modifierDictionary
                .Where(x => x.Value[Constants.TYPE].Equals(Constants.INPUT))
                .Any(x => !string.IsNullOrEmpty(x.Value[Constants.VALUE]));
        }

        public static int GetFirstUnsetInput(this Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Get index of first unset input modifier
            return modifierDictionary
                .Where(kvp => kvp.Value[Constants.TYPE]
                    .Equals(Constants.INPUT))
                .OrderBy(kvp => kvp.Key)
                .FirstOrDefault(kvp =>
                    kvp.Value[Constants.VALUE].Length
                    != int.Parse(kvp.Value[Constants.LENGTH]))
                .Key;
        }

        public static int GetLastSetInput(this Dictionary<int, Dictionary<string, string>> modifierDictionary)
        {
            // Get index of Last set input modifier
            return modifierDictionary
                .Where(kvp => kvp.Value[Constants.TYPE]
                    .Equals(Constants.INPUT))
                .OrderBy(kvp => kvp.Key)
                .LastOrDefault(kvp =>
                    kvp.Value[Constants.VALUE].Length
                        .Equals(int.Parse(kvp.Value[Constants.LENGTH])))
                .Key;
        }



        public static void WipeLastInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
            // Remove last user input
            try
            {
                int index = dict.GetModifierIndex();

                // Switch to previous input field if current one is empty
                if (dict[index][Constants.VALUE].Length == 0)
                {
                    index = dict.GetLastSetInput();
                }

                string value = dict[index][Constants.VALUE];
                // TODO: verify behaviour when approaching index 0
                dict[index][Constants.VALUE] = value[..^1];
            }
            catch (KeyNotFoundException) { /* DO NOTHING */ }
        }

        public static int GetModifierIndex(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict.GetFirstUnsetInput() == 0 
                ? dict.GetLastSetInput()
                : dict.GetFirstUnsetInput();
        }
    }
}
