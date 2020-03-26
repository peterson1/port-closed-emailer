using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib.StringTools
{
    public static class StringExtensions
    {
        public static string Before(this string text, string findThis, bool seekFromEnd = false)
        {
            var pos = seekFromEnd ? text.LastIndexOf(findThis)
                                  : text.IndexOf(findThis);
            if (pos == -1) return text;
            return text.Substring(0, pos);
        }


        public static string RemoveExtraSpaces(this string text)
        {
            if (text.IsBlank()) return text;
            return Regex.Replace(text, @"\s+", " ").Trim();
        }


        public static string After(this string text, string findThis, bool seekFromEnd = false)
        {
            var pos = seekFromEnd ? text.LastIndexOf(findThis)
                                  : text.IndexOf(findThis);
            if (pos == -1) return text;
            return text.Substring(pos + findThis.Length);
        }


        public static string Between(this string fullText,
                    string firstString, string lastString,
                    bool seekLastStringFromEnd = false)
        {
            if (fullText.IsBlank()) return string.Empty;

            var pos1 = fullText.IndexOf(firstString);
            if (pos1 == -1) return string.Empty;
            pos1 += firstString.Length;

            var pos2 = seekLastStringFromEnd ?
                fullText.LastIndexOf(lastString)
                : fullText.IndexOf(lastString, pos1);

            if (pos2 == -1 || pos2 <= pos1)
                return fullText.Substring(pos1);

            return fullText.Substring(pos1, pos2 - pos1);
        }


        public static string AppendIfNotEndingWith(this string text, string suffix)
            => text.EndsWith(suffix) ? text : text + suffix;


        public static bool IsBlank(this string text)
        {
            if (text == null) return true;
            return string.IsNullOrWhiteSpace(text);
        }


        public static string NullIfBlank(this string text)
            => text.IsBlank() ? null : text;


        public static string JoinNonBlanks(this string separator, params string[] texts)
        {
            var nonBlanks = new List<string>();
            foreach (var text in texts)
            {
                var trimd = text?.Trim();
                if (!trimd.IsBlank())
                    nonBlanks.Add(trimd);
            }
            return string.Join(separator, nonBlanks);
        }


        public static bool HasText(this string lookInHere, string findThis)
        {
            var allLength = lookInHere.Length;
            var subLength = findThis.Length;
            var difLength = lookInHere.Replace(findThis, String.Empty).Length;
            return (allLength - difLength) / subLength > 0;
        }


        //https://stackoverflow.com/a/26558102/3973863
        public static string SHA1ForUTF8(this string utf8Text)
        {
            if (utf8Text.IsBlank()) return string.Empty;
            var byts = Encoding.UTF8.GetBytes(utf8Text);
            var hash = new SHA1Managed().ComputeHash(byts);
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }


        public static string MD5ForUTF8(this string utf8Text)
        {
            if (utf8Text.IsBlank()) return string.Empty;
            var byts = Encoding.UTF8.GetBytes(utf8Text);
            var hash = MD5.Create().ComputeHash(byts);
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }


        public static List<string> SplitTrim(this string text, string separator)
        {
            var split = text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return split.Select(x => x.Trim()).ToList();
        }


        public static string[] SplitByLine(this string text,
            StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
                => text.IsBlank() ? new string[] { }
                    : text.Split(new[] { "\r\n", "\r", "\n" }, splitOptions);


        //https://stackoverflow.com/a/11743162/3973863
        public static string EncodeBase64(this string text)
        {
            var byts = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(byts);
        }


        public static string DecodeBase64(this string text)
        {
            if (text.IsBlank()) return text;
            byte[] byts = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(byts);
        }


        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                     || base64String.Contains(" ") || base64String.Contains("\t")
                     || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch { }
            return false;
        }




        private static Nullable<T> ToNullableEnum<T>(this string value)
            where T : struct
        {
            var defaultValue = (Nullable<T>)null;
            if (value.IsBlank()) return defaultValue;
            return Enum.TryParse(value, out T parsd) ? parsd : defaultValue;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            var val = value.ToNullableEnum<T>();
            return val.HasValue ? val.Value
                : throw new InvalidCastException(
                    $"“{value}” cannot be converted to {Typ.Nme<T>()}");
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
            => value.ToNullableEnum<T>() ?? defaultValue;



        public static string ToTitleCase(this string text)
            //=> System.Threading.Thread.CurrentThread.CurrentCulture
            => CultureInfo.InvariantCulture
                .TextInfo.ToTitleCase(text.ToLower());


        // https://stackoverflow.com/a/2776689
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


        //https://codepattern.net/Blog/post/how-to-convert-utf7-string-into-ansi-in-csharp
        public static string DecodeUTF7(this string utf7String)
        {
            var byts = Encoding.Default.GetBytes(utf7String);
            return Encoding.UTF7.GetString(byts);
        }


        //https://stackoverflow.com/a/5238289/3973863
        public static MemoryStream ToUTF8Stream(this string text) 
            => new MemoryStream(Encoding.UTF8.GetBytes(text ?? ""));
    }
}
