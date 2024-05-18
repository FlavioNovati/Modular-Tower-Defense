using System.Collections.Generic;

public static class ListExtentions
{
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static List<T> RemoveLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
        return list;
    }
}
