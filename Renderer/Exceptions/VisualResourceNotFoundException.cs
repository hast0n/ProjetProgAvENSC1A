using System;
using System.Collections.Generic;
using System.Text;

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
}
