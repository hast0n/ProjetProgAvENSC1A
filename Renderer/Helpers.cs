﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProjetProgAvENSC1A.Services;

namespace CliLayoutRenderTools
{
    // A simple class used to help the Renderer to frame content
    static class RendererExtensions
    {
        /// <summary>
        /// Left pad a string.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="padding"></param>
        /// <param name="removeOne"></param>
        /// <returns></returns>
        public static string Pad(this string line, int padding, bool removeOne)
        {
            return $"{new string(' ', padding - (removeOne ? 1 : 0))}{line}";
        }

        /// <summary>
        /// Returns int parity.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }


        /// <summary>
        /// Return a bool indicating if user has already input data.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool HasSetModifiers(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict
                .Where(x => x.Value[Constants.TYPE].Equals(Constants.INPUT))
                .Any(x => !string.IsNullOrEmpty(x.Value[Constants.VALUE]));
        }

        /// <summary>
        /// Returns index of first unset input modifier.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static int GetFirstUnsetInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict
                .Where(kvp => kvp.Value[Constants.TYPE].Equals(Constants.INPUT))
                .OrderBy(kvp => kvp.Key)
                .FirstOrDefault(kvp =>
                    kvp.Value[Constants.VALUE].Length != int.Parse(kvp.Value[Constants.LENGTH]))
                .Key;
        }

        /// <summary>
        /// Returns index of Last set input modifier.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static int GetLastSetInput(this Dictionary<int, Dictionary<string, string>> dict)
        {
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
            return dict.Where(kvp => 
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                .TakeWhile(kvp =>
                    kvp.Key < index)
                .LastOrDefault().Key;
        }

        public static int GetNextSelectorIndex(this Dictionary<int, Dictionary<string, string>> dict, int index)
        {
            return dict.Where(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                .SkipWhile(kvp =>
                    kvp.Key <= index)
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
                if (dict[index][Constants.VALUE].TrimEnd().Length == 0)
                {
                    index = dict.GetLastSetInput();
                }

                string value = dict[index][Constants.VALUE];
                int totalLength = int.Parse(dict[index][Constants.LENGTH]);

                if (value.Length.Equals(totalLength)) value = value.TrimEnd();
                
                if (value.Length > 0) dict[index][Constants.VALUE] = value[..^1];
            }
            catch (KeyNotFoundException) { /* DO NOTHING */ }
        }

        public static int GetInputFieldIndex(this Dictionary<int, Dictionary<string, string>> dict)
        {
            var m = dict.GetFirstUnsetInput();
            return m == 0 ? dict.GetLastSetInput() : m;
        }
        
        public static bool ContainsField(this Dictionary<int, Dictionary<string, string>> dict, string fieldType)
        {
            return dict.Any(kvp => kvp.Value[Constants.TYPE].Equals(fieldType));
        }


        public static bool IsReadyForReturn(this Dictionary<int, Dictionary<string, string>> dict)
        {
            return dict.Where(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT))
                .All(kvp =>
                    kvp.Value[Constants.VALUE].Length
                    .Equals(int.Parse(kvp.Value[Constants.LENGTH])));
        }

        public static string GetSelectedValue(this ImmutableDictionary<int, Dictionary<string, string>> input)
        {
            try
            {
                return input.FirstOrDefault(kvp =>
                    kvp.Value[Constants.SELECTED] == bool.TrueString)
                    .Value[Constants.INDEX];
            }
            catch (KeyNotFoundException) // TODO: set correct expression to catch
            {
                return null;
            }
        }

        public static List<string> GetUserInputs(this ImmutableDictionary<int, Dictionary<string, string>> dict)
        {
            List<string> inputs = new List<string>();

            foreach (var keyValuePair in dict.Where(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)))
            {
                inputs.Add(keyValuePair.Value[Constants.VALUE]);
            }

            return inputs;
        }

        public static void TrimInputFields(this Dictionary<int, Dictionary<string, string>> dict)
        {
            foreach (var keyValuePair in dict.Where(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)))
            {
                string value = keyValuePair.Value[Constants.VALUE];
                keyValuePair.Value[Constants.VALUE] = value.TrimEnd();
            }
        }
    }
}
