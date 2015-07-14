using System;

namespace Bardock.R2O
{
    public class ConverterException : Exception
    {
        public ConverterException(string msg = null)
            : base(msg)
        { }
    }

    public class ConverterRepeatedColumnException : ConverterException
    {
        public ConverterRepeatedColumnException(string column)
            : base(String.Format("Converted object already has a property with name {0}", column))
        { }
    }
}