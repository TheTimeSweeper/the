using System;
using UnityEngine;
namespace Matchmaker
{
    public static class Log
    {
        public static void Warning(object nip, UnityEngine.Object nop = null)
        {
            UnityEngine.Debug.LogWarning(nop, nop);
        }

        internal static void Debug(object v, UnityEngine.Object nop = null)
        {
            UnityEngine.Debug.Log(v, nop);
        }

        internal static void Error(object v, UnityEngine.Object nop = null)
        {
            UnityEngine.Debug.LogError(v, nop);
        }
    }
}
