using System;

public static class ExtensionMethods
{
    //Case insensitive string comparison extension method
    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
    }
}