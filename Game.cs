using System;
using System.Threading;
using TeoEngine.Private;
using LibKatana;

namespace TeoEngine
{
    public static class Game
    {
        public static bool EngineStarted { get; private set; } = false;
        public static IntVector2 ScreenSize { get; private set; }
        public static short CharacterSize { get; private set; }
        public static string Title { get => Console.Title; set => Console.Title = value; }
        public static char transparentCharacter = '█';
        public static Visibility Background = new();

        //private static bool ptitleBar = true;
        //public static bool TitleBar { get => ptitleBar; set { ptitleBar = value; Terminal.TitlebarVisible = value; } }
        public static bool Output { get => Draw.outPut; set => Draw.outPut = value; }
        public static bool DelayFrames = false;
        public static bool AutoGarbageCollect = false;
        public static bool IntDraw = false;

        public static Point Camera = new();

        //  Represent the number of caracters that need to be different from preview frame to switch from drawing only those to fullScreen draw
        public static long elementsChanged = 300;


        public static void SetWindowSize(int width, int heigth, short characterSize)
        {
            CharacterSize = characterSize;
            try
            {
                Console.CursorVisible = false;
                Console.WindowWidth = width;
                Console.WindowHeight = heigth;
                Console.SetBufferSize(width, heigth);
                ScreenSize = new(Console.WindowWidth, Console.WindowHeight);
                Terminal.SetFontSize(Game.CharacterSize, Game.CharacterSize);
                Terminal.ScrollbarVisible = false;
                Terminal.DisableResize();
            } catch (Exception e) {
                throw new Exception("Screen size error: " + e.ToString());
            }
        }


        public static void Initialize(Action userStart, Action userUpdate)
        {
            if (EngineStarted == true) return;

            ////    Start
            EngineStarted = true;
            Console.Clear();
            SetWindowSize(128, 40, 12);
            Camera.ParentOrder = 0;
            //Terminal.CanClose = false;
            //Terminal.TitlebarVisible = false;
            Thread InputControl = new(Input.Start);
            InputControl.Start();
            Time.Start();
            userStart?.Invoke();
            Draw.Start();


            ////    Update
            while (true)
            {
                Time.TimeControl();
                userUpdate?.Invoke();
                Input.EveryFrame();
                Application.Update();
                Draw.Update();

                if (AutoGarbageCollect && Time.FirstFrameInSecond())
                    GC.Collect();

                if (DelayFrames)
                    Thread.Sleep(12);
            }
        }


        public static void Exit()
        {
            Console.Clear();
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}
