using CommonLib.StringTools;
using System;

namespace CommonLib.ExceptionTools
{
    public static class Unsupported
    {
        public static ArgumentException Arg<T>(T arg) 
            => new ArgumentException($"Invalid {Typ.Nme<T>()}: [{arg}]");
    }
}
