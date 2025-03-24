using System;
using System.Diagnostics;

namespace TeoEngine
{
    public static class Time
    {
        private static long previewSecond = 0;
        private static double previewMilisecond = 0;
        private static double previewFramesCount = 0;
        private static bool newSecond = false;

        public static double TimeSeconds { get => ts.TotalSeconds; }
        public static int FrameCount { get; private set; } = 0;
        public static double DeltaTime { get; private set; } = 0;
        public static double FramesPerSecond { get; private set; } = 0;

        static readonly Stopwatch stopWatch = new();
        static TimeSpan ts;

        internal static void Start() => stopWatch.Start();

        internal static void TimeControl()
        {
            newSecond = false;
            ts = stopWatch.Elapsed;
            if ((long)ts.TotalSeconds != previewSecond)    ////    EVERY SECOND
            {
                newSecond = true;
                previewSecond = (long)ts.TotalSeconds;
                FramesPerSecond = FrameCount - previewFramesCount;
                previewFramesCount = FrameCount;
                //Console.Title = (1 / DeltaTime).ToString() + "    " + FramesPerSecond.ToString();
            }
            FrameCount++;

            DeltaTime = (ts.Milliseconds - previewMilisecond + 1000) % 1000 / 1000;
            previewMilisecond = ts.Milliseconds;
        }

        public static bool FirstFrameInSecond() => newSecond;
        
        public static bool FirstFrameInSecond(long second) => newSecond && (long)ts.TotalSeconds == second;
    }
}
