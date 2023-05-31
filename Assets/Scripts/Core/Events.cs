using System;

namespace Core
{
    public static class Events
    {
        public static event Action Begin;
        public static event Action End;
        public static event Action Restart;
        
        public static void InvokeBegin() => Begin?.Invoke();
        public static void InvokeEnd() => End?.Invoke();
        public static void InvokeRestart() => Restart?.Invoke();
    }
}