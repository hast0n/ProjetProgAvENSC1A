using System;
using System.Collections.Generic;
using System.Text;

namespace CliLayoutRenderTools
{
    public class AttributeValueNotFoundException : Exception
    {
        public AttributeValueNotFoundException()
        {

        }

        public AttributeValueNotFoundException(string message)
            : base(message)
        {

        }

        public AttributeValueNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
