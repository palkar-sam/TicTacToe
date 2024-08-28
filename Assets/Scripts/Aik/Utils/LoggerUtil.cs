using UnityEngine;

namespace Aik.Utils
{
    public class LoggerUtil
    {
        public static bool logEnabled = true;
        public static void Log(string logStr)
        {
            if (logEnabled)
                Debug.Log(logStr);
        }

        public static void LogException(System.Exception message)
        {
            if (logEnabled)
                Debug.LogException(message);
        }

        public static void LogError(string logStr)
        {
            if (logEnabled)
                Debug.LogError(logStr);
        }
    }
}