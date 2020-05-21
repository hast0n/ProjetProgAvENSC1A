using System;
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
    }


    public class ModifierDictionary : Dictionary<int, Dictionary<string, string>>
    {
        // Inputs
        public int GetPreviousInputFieldIndex(int index)
        {
            return this.LastOrDefault(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)
                && kvp.Key < index).Key;
        }

        public int GetNextInputFieldIndex(int index)
        {
            return this.FirstOrDefault(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)
                && kvp.Key > index).Key;
        }

        public void TrimInputFields()
        {
            foreach (var keyValuePair in this.Where(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)))
            {
                string value = keyValuePair.Value[Constants.VALUE];
                keyValuePair.Value[Constants.VALUE] = value.TrimEnd();
            }
        }

        public List<string> GetUserInputs()
        {
            List<string> inputs = new List<string>();

            foreach (var keyValuePair in this.Where(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.INPUT)))
            {
                inputs.Add(keyValuePair.Value[Constants.VALUE]);
            }

            return inputs;
        }
        
        // Selectors
        public int GetSelectedIndex()
        {
            return this.FirstOrDefault(kvp =>
                kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR) &&
                kvp.Value[Constants.SELECTED].Equals(bool.TrueString)).Key;
        }

        public int GetPreviousSelectorIndex(int index)
        {
            return this.Where(kvp => 
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                .TakeWhile(kvp =>
                    kvp.Key < index)
                .LastOrDefault().Key;
        }

        public int GetNextSelectorIndex(int index)
        {
            return this.Where(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                .SkipWhile(kvp =>
                    kvp.Key <= index)
                .FirstOrDefault().Key;
        }

        public void RemoveSelectedDuplicate()
        {
            int index = this.GetSelectedIndex();

            
            // Here we assume 0 is the default null value
            // 0 could be a valid value but will most likely not be

            // TODO: Change null value for modifierDictionary

            // We also assume that this contains a selector field as 
            // this has passed through ContainsField() before entering this scope

            // Here we set the first selector field to selected
            if (index == 0)
            {
                this.First(kvp => kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR))
                    .Value[Constants.SELECTED] = bool.TrueString;

                index = this.GetSelectedIndex();
            }

            foreach (var kvp in this.Where(kvp => 
                kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR)
                && kvp.Value[Constants.SELECTED].Equals(bool.TrueString)
                && !kvp.Key.Equals(index)))
                
                kvp.Value[Constants.SELECTED] = bool.FalseString;
        }

        public string GetSelectedValue()
        {
            try
            {
                return this.FirstOrDefault(kvp =>
                    kvp.Value[Constants.TYPE].Equals(Constants.SELECTOR)
                    && kvp.Value[Constants.SELECTED] == bool.TrueString)
                    .Value[Constants.INDEX];
            }
            catch (Exception) // catch KeyNotFoundException, NullReferenceException
            {
                return null;
            }
        }

        // Any
        public bool ContainsField(string fieldType)
        {
            return this.Any(kvp => kvp.Value[Constants.TYPE].Equals(fieldType));
        }
    }
}
