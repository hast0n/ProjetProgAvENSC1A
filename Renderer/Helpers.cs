using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CliLayoutRenderTools
{
    public class VisualResourceNotFoundException : Exception
    {
        public VisualResourceNotFoundException()
        {

        }

        public VisualResourceNotFoundException(string message)
            : base(message)
        {

        }

        public VisualResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    // A simple class used to help the Renderer to frame content
    static class RendererHelper
    {
        public static string Pad(this string line, int padding)
        {
            return $"{new string(' ', padding)}{line}";
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }
    }
}
