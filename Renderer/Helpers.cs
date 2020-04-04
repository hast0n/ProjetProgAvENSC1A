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



        public static bool HasSetModifiers(this Dictionary<int, Dictionary<string, string>> dict)
        {
            // Return a bool indicating if user has already input data
            return dict
                .Where(x => x.Value[Constants.TYPE].Equals(Constants.INPUT))
                .Any(x => !string.IsNullOrEmpty(x.Value[Constants.VALUE]));
        }

        public static int GetFirstUnsetInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
            // Get index of first unset input modifier
            return dict
                .Where(kvp => kvp.Value[Constants.TYPE]
                    .Equals(Constants.INPUT))
                .OrderBy(kvp => kvp.Key)
                .FirstOrDefault(kvp =>
                    kvp.Value[Constants.VALUE].Length
                    != int.Parse(kvp.Value[Constants.LENGTH]))
                .Key;
        }

        public static int GetLastSetInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
            // Get index of Last set input modifier
            return dict
                .Where(kvp => kvp.Value[Constants.TYPE]
                    .Equals(Constants.INPUT))
                .OrderBy(kvp => kvp.Key)
                .LastOrDefault(kvp =>
                    kvp.Value[Constants.VALUE].Length
                        .Equals(int.Parse(kvp.Value[Constants.LENGTH])))
                .Key;
        }



        public static int GetSelectedIndex(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict.FirstOrDefault(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR) &&
                kvp.Value[Constants.SELECTED].Equals(bool.TrueString)).Key;
        }

        public static int GetPreviousSelectorIndex(this Dictionary<int, Dictionary<string, string>> dict, int index)
        {
            return dict.TakeWhile(kvp => 
                kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR) 
                && kvp.Key < index)
                .LastOrDefault().Key;
        }

        public static int GetNextSelectorIndex(this Dictionary<int, Dictionary<string, string>> dict, int index)
        {
            return dict.SkipWhile(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR)
                    && kvp.Key <= index)
                .FirstOrDefault().Key;
        }

        public static void RemoveSelectedDuplicate(this Dictionary<int, Dictionary<string, string>> dict)
        {
            int index = GetSelectedIndex(dict);

            
            // Here we assume 0 is the default null value
            // 0 could be a valid value but will most likely not be

            // TODO: Change null value for modifierDictionary

            // We also assume that dict contains a selector field as 
            // dict has passed through ContainsField() before entering this scope

            // Here we set the first selector field to selected
            if (index == 0)
            {
                dict.First(kvp => kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                    .Value[Constants.SELECTED] = bool.TrueString;

                index = GetSelectedIndex(dict);
            }

            foreach (var kvp in dict.Where(kvp => 
                kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR)
                && kvp.Value[Constants.SELECTED].Equals(bool.TrueString)
                && !kvp.Key.Equals(index)))
                
                kvp.Value[Constants.SELECTED] = bool.FalseString;
        }


        public static void WipeLastInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
            // Remove last user input
            try
            {
                int index = dict.GetInputFieldIndex();

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

        public static int GetInputFieldIndex(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict.GetFirstUnsetInput() == 0 
                ? dict.GetLastSetInput()
                : dict.GetFirstUnsetInput();
        }
        
        public static bool ContainsField(this Dictionary<int, Dictionary<string, string>> dict, string fieldType)
        {
            return dict.Any(kvp => kvp.Value[Constants.TYPE].Equals(fieldType));
        }
    }
}
