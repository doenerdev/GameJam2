using UnityEngine;
using System.Collections;

public static class GenericsHelper {

    public static int Test;

    public static bool Compare<T>(T x, T y) where T : class
    {
        return x == y;
    }
}
