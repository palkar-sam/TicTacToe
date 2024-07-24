using UnityEngine;

namespace Utils
{
    public class LoggerUtil
    {
        public static bool logEnabled = true;
       public static void Log(string logStr)
        {
            if(logEnabled)
                Debug.Log(logStr);
        }
    }
}