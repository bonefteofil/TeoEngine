using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices.Marshalling;

namespace TeoEngine
{
    public class Input
    {
        private static readonly byte[] keys = new byte[256];
        private static readonly byte[] clicks = new byte[3];
        private static int input = 0;
        private static byte previewKeysPressed = 0;

        public static byte KeysPressed { get; private set; } = 0;
        public static Vector2 MousePosition { get; private set; } = new();
        public static short MouseScroll { get; private set; } = 0;

        internal static void Start()
        {
            Console.TreatControlCAsInput = true;
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            var record = new NativeMethods.INPUT_RECORD();
            uint recordLen = 0;
            int mode = 0;

            if (!NativeMethods.GetConsoleMode(handle, ref mode))
                throw new Win32Exception();

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!NativeMethods.SetConsoleMode(handle, mode))
                throw new Win32Exception();

            while (true)
            {
                if (!NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))
                    throw new Win32Exception();
                //pressed = (ConsoleKey)Enum.ToObject(typeof(ConsoleKey), record.KeyEvent.wVirtualKeyCode);

                if (record.EventType == NativeMethods.MOUSE_EVENT)
                {
                    input = record.MouseEvent.dwButtonState;
                    if (input <= -7864320 + 7) MouseScroll = -1;
                    if (input >= 7864320) MouseScroll = 1;
                    input = (input + 7864320) % 7864320;
                    MousePosition = new Vector2(record.MouseEvent.dwMousePosition.x, record.MouseEvent.dwMousePosition.y) - Game.ScreenSize / 2;
                }
                if (record.EventType == NativeMethods.KEY_EVENT)
                {
                    input = record.KeyEvent.wVirtualKeyCode;
                    if (!record.KeyEvent.bKeyDown)
                        keys[input] = 4;
                    else if (keys[input] == 0)
                        keys[input] = 1;
                }
            }
            /*
            POSIBILITES
            Console.WriteLine("Mouse event");
            Console.WriteLine(string.Format("    X ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.X));
            Console.WriteLine(string.Format("    Y ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.Y));
            Console.WriteLine(string.Format("    dwButtonState ...: 0x{0:X4}  ", record.MouseEvent.dwButtonState));
            Console.WriteLine(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.MouseEvent.dwControlKeyState));
            Console.WriteLine(string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.dwEventFlags));
            Console.WriteLine("Key event  ");
            Console.WriteLine(string.Format("    bKeyDown  .......:  {0,5}  ", record.KeyEvent.bKeyDown));
            Console.WriteLine(string.Format("    wRepeatCount ....:   {0,4:0}  ", record.KeyEvent.wRepeatCount));
            Console.WriteLine(string.Format("    wVirtualKeyCode .:   {0,4:0}  ", record.KeyEvent.wVirtualKeyCode));
            Console.WriteLine(string.Format("    uChar ...........:      {0}  ", record.KeyEvent.UnicodeChar));
            Console.WriteLine(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.KeyEvent.dwControlKeyState));
            */
        }

        internal static void EveryFrame()
        {
            previewKeysPressed = KeysPressed;
            KeysPressed = 0;
            int i;
            for (i = 0; i < keys.Length; i++)
            {
                if (keys[i] != 0 && keys[i] != 3)
                    keys[i] = (byte)(++keys[i] % 6);
                if (keys[i] > 2)
                    KeysPressed++;
            }

            MouseScroll = 0;
            for (i = 0; i < 3; i++) // CHANGE FROM DOWN AND UP CLICKS TO PRESSED AND UNPRESSED
            {
                if (clicks[i] == 1) clicks[i] = 2;
                if (clicks[i] == 3) clicks[i] = 0;
            }
            i = 2; // VERIFY PRESSED CLICKS
            if (input >= 4)
            {
                if (clicks[i] == 0 || clicks[i] == 3)
                    clicks[i] = 1;
            }
            else
            {
                if (clicks[i] == 1 || clicks[i] == 2)
                    clicks[i] = 3;
            }
            i--;
            if (input % 4 >= 2)
            {
                if (clicks[i] == 0 || clicks[i] == 3)
                    clicks[i] = 1;
            }
            else
            {
                if (clicks[i] == 1 || clicks[i] == 2)
                    clicks[i] = 3;
            }
            i--;
            if (input % 2 == 1)
            {
                if (clicks[i] == 0 || clicks[i] == 3)
                    clicks[i] = 1;
            }
            else
            {
                if (clicks[i] == 1 || clicks[i] == 2)
                    clicks[i] = 3;
            }
        }

        public static bool GetKeyPressed(ConsoleKey key) => keys[(int)key] > 1;

        public static bool GetKeyDown(ConsoleKey key) => keys[(int)key] == 2;

        public static bool GetKeyUp(ConsoleKey key) => keys[(int)key] == 5;

        public static bool GetMouseButtonPressed(byte button) => clicks[button] == 1 || clicks[button] == 2;

        public static bool GetMouseButtonDown(byte button) => clicks[button] == 1;

        public static bool GetMouseButtonUp(byte button) => clicks[button] == 3;

        public static bool PressAnyKey() => KeysPressed > previewKeysPressed;



        private class NativeMethods
        {
            public const int STD_INPUT_HANDLE = -10;

            public const int ENABLE_MOUSE_INPUT = 0x0010;
            public const int ENABLE_QUICK_EDIT_MODE = 0x0040;
            public const int ENABLE_EXTENDED_FLAGS = 0x0080;

            public const int KEY_EVENT = 1;
            public const int MOUSE_EVENT = 2;


            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT_RECORD
            {
                [FieldOffset(0)]
                public short EventType;
                [FieldOffset(4)]
                public KEY_EVENT_RECORD KeyEvent;
                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;
            }

            public struct MOUSE_EVENT_RECORD
            {
                public COORD dwMousePosition;
                public int dwButtonState;
                public int dwControlKeyState;
                public int dwEventFlags;
            }

            public struct COORD
            {
                public ushort x;
                public ushort y;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct KEY_EVENT_RECORD
            {
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.Bool)]
                public bool bKeyDown;
                [FieldOffset(4)]
                public ushort wRepeatCount;
                [FieldOffset(6)]
                public ushort wVirtualKeyCode;
                [FieldOffset(8)]
                public ushort wVirtualScanCode;
                [FieldOffset(10)]
                public char UnicodeChar;
                [FieldOffset(10)]
                public byte AsciiChar;
                [FieldOffset(12)]
                public int dwControlKeyState;
            };


            public class ConsoleHandle : SafeHandleMinusOneIsInvalid
            {
                public ConsoleHandle() : base(false) { }

                protected override bool ReleaseHandle()
                {
                    return true;
                }
            }


            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetConsoleMode(ConsoleHandle hConsoleHandle, ref int lpMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern ConsoleHandle GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

            // [LibraryImport("kernel32.dll", SetLastError = true)]
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetConsoleMode(ConsoleHandle hConsoleHandle, int dwMode);
        }
    }
}
