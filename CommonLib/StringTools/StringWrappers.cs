namespace CommonLib.StringTools
{
    public static class StringWrappers
    {
        public static string Quotify(this string text) => $"“{text}”";
    }


    public static class Typ
    {
        public static string Nme<T>() => $"‹{typeof(T).Name}›";
        public static string Nme(object obj) => $"‹{obj.GetType().Name}›";
    }
}
