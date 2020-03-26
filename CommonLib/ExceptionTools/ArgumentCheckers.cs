using CommonLib.StringTools;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommonLib.ExceptionTools
{
    public static class ArgumentCheckers
    {
        public static T ThrowIfNull<T>(this T obj, [CallerFilePath] string filePath = null, [CallerMemberName] string method = null, [CallerLineNumber] int? lineNum = null)
        {
            if (obj != null) return obj;
            var file = Path.GetFileNameWithoutExtension(filePath);
            var msg  = $"{file}:{method}:{lineNum}";
            throw new ArgumentNullException($"‹{typeof(T).Name}›", msg);
        }


        public static string ThrowIfBlank(this string text, [CallerFilePath] string filePath = null, [CallerMemberName] string method = null, [CallerLineNumber] int? lineNum = null)
        {
            if (!text.IsBlank()) return text;
            var file = Path.GetFileNameWithoutExtension(filePath);
            var src  = $"{file}:{method}:{lineNum}";
            var msg  = "String should not be blank.";
            throw new ArgumentException($"{msg}{L.f}{src}");
        }
    }
}
