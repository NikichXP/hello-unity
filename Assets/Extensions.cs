using System.Collections.Generic;

public static class Extensions
{
    public static TV GetOrPut<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue)
    {
        TV value;
        if (dict.TryGetValue(key, out value))
        {
            return value;
        }
        dict[key] = defaultValue;
        return defaultValue;
    }
}