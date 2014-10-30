using UnityEngine;

// ReSharper disable once CheckNamespace
public class Script : MonoBehaviour
{
    public static void LogFormat(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}