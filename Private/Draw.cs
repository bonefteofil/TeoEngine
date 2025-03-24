using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace TeoEngine.Private
{
    static class Draw
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord(short X, short Y)
        {
            public short X = X;
            public short Y = Y;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        internal static bool outPut = true;
        private static int elementsChanges;
        private static readonly short maxWidth = 1200;
        private static readonly short maxHeigth = 800;
        public static readonly char[,] drawBuffer = new char[maxWidth, maxHeigth];
        private static readonly char[,] previewBuffer = new char[maxWidth, maxHeigth];
        public static readonly byte[,] color = new byte[maxWidth, maxHeigth];
        private static readonly byte[,] previewColor = new byte[maxWidth, maxHeigth];
        private static readonly CharInfo[] fastBuffer = new CharInfo[maxWidth * maxHeigth];



        public static void DrawPoligon(Vector2[] Points, Vector2 Position, Visibility Elem)
        {
            if (Points.Length == 0) return;

            double pixelX, pixelY;
            int i, j, nodes;
            int ymin = (int)Points[0].Y;
            int ymax = ymin;
            int xmin = (int)Points[0].X;
            int xmax = xmin;

            for (i = 0; i < Points.Length; i++) // Area of drawing
            {
                ymin = Math.Min(ymin, (int)Points[i].Y - 10);
                ymax = Math.Max(ymax, (int)Points[i].Y + 10);
                xmin = Math.Min(xmin, (int)Points[i].X - 10);
                xmax = Math.Max(xmax, (int)Points[i].X + 10);
            }

            double[] nodeX = new double[Points.Length];

            // Loop through the rows of the image.
            for (pixelY = ymin; pixelY <= ymax; pixelY++)
            {
                // Build a list of nodes.
                nodes = 0;
                j = Points.Length - 1;
                for (i = 0; i < Points.Length; i++)
                {
                    if (Points[i].Y < pixelY && Points[j].Y >= pixelY || Points[j].Y < pixelY && Points[i].Y >= pixelY)
                    {
                        nodeX[nodes++] = (Points[i].X + (pixelY - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X));
                    }
                    j = i;
                }

                // Sort the nodes, via a simple “Bubble” sort.
                i = 0;
                while (i < nodes - 1)
                {
                    if (nodeX[i] > nodeX[i + 1])
                    {
                        nodeX[i] += nodeX[i + 1];
                        nodeX[i + 1] = nodeX[i] - nodeX[i + 1];
                        nodeX[i] -= nodeX[i + 1];
                        if (i > 0)
                            i--;
                    }
                    else
                        i++;
                }
                // Fill the pixels between node pairs.
                for (i = 0; i < nodes; i += 2)
                {
                    if (nodeX[i] >= xmax)
                        break;
                    if (nodeX[i + 1] > xmin)
                    {
                        if (nodeX[i] < xmin)
                            nodeX[i] = xmin;
                        if (nodeX[i + 1] > xmax)
                            nodeX[i + 1] = xmax;
                        for (pixelX = (int)nodeX[i]; pixelX < nodeX[i + 1]; pixelX++)
                        {
                            int x = (int)(pixelX + Position.X);
                            int y = (int)(pixelY + Position.Y);
                            Draw.DrawCharactert(Elem.character, x, y, Elem);
                        }
                    }
                }
            }
        }


        public static void DrawCharactert(char character, int x, int y, Visibility view)
        {
            if (x >= 0 && x < Game.ScreenSize.X && y >= 0 && y < Game.ScreenSize.Y)
            {
                drawBuffer[x, y] = character;
                byte cl;
                if (!view.transparentColor)
                    cl = (byte)view.color;
                else
                    cl = (byte)(Draw.color[x, y] % 16);
                if (!view.transparentBgColor)
                    cl += (byte)((byte)view.bgColor * 16);
                else
                    cl += (byte)(Draw.color[x, y] / 16 * 16);
                color[x, y] = cl;
            }
        }

        public static void Update()
        {
            if (outPut)
            {
                elementsChanges = 0;
                DrawGameObjects_OnBuffer();
                DrawUIElemens_OnBuffer();
                Fast_Draw();
            }
        }

        public static void Start()
        {
            for (int i = 0; i < Game.ScreenSize.X; i++)
            {
                for (int j = 0; j < Game.ScreenSize.Y; j++)
                {
                    drawBuffer[i, j] = Game.Background.character;
                    color[i, j] = (byte)((byte)Game.Background.color + (byte)Game.Background.bgColor * 16);
                }
            }
        }

        private static void DrawGameObjects_OnBuffer()
        {
            foreach (Element obj in Application.Objects)
                obj.DrawOnBuffer(ObjectSpace.World);
        }

        private static void DrawUIElemens_OnBuffer()
        {
            for (int i = 0; i < Application.UIElements.Count; i++)
                Application.UIElements[i].DrawOnBuffer(ObjectSpace.Screen);
            /*foreach (UIElement elem in Application.UIElements)
                elem.DrawOnBuffer(ObjectSpace.Screen);
            */
        }

        private static void Fast_Draw()
        {
            for (int j = 0; j < Game.ScreenSize.Y; j++)
            {
                for (int i = 0; i < Game.ScreenSize.X; i++)
                {
                    if (drawBuffer[i, j] != previewBuffer[i, j] || color[i, j] != previewColor[i, j])
                        elementsChanges++;
                    fastBuffer[i + j * Game.ScreenSize.X].Char.AsciiChar = (byte)drawBuffer[i, j];
                    fastBuffer[i + j * Game.ScreenSize.X].Attributes = color[i, j];
                }
            }
            if (elementsChanges > Game.elementsChanged)
            {
                SmallRect canvas = new() { Left = 0, Top = 0, Right = 80, Bottom = 25 };
                SafeFileHandle newFile = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
                canvas.Right = (short)Game.ScreenSize.X;
                canvas.Bottom = (short)Game.ScreenSize.Y;
                bool b = WriteConsoleOutput(newFile, fastBuffer,
                  new Coord() { X = (short)Game.ScreenSize.X, Y = (short)Game.ScreenSize.Y },
                  new Coord() { X = 0, Y = 0 },
                  ref canvas);
                if (!b)
                    Game.Exit();
                for (int j = 0; j < Game.ScreenSize.Y; j++)
                {
                    for (int i = 0; i < Game.ScreenSize.X; i++)
                    {
                        previewBuffer[i, j] = drawBuffer[i, j];
                        previewColor[i, j] = color[i, j];
                        drawBuffer[i, j] = Game.Background.character;
                        color[i, j] = (byte)((byte)Game.Background.color + (byte)Game.Background.bgColor * 16);
                    }
                }
            }
            else
                DrawBuffer_OnScreen();
        }

        private static void DrawBuffer_OnScreen()
        {
            for (int j = 0; j < Game.ScreenSize.Y; j++)
            {
                for (int i = 0; i < Game.ScreenSize.X; i++)
                {
                    if (i == Game.ScreenSize.X - 1 && j == Game.ScreenSize.Y - 1)
                        break;
                    if (drawBuffer[i, j] != previewBuffer[i, j] || color[i, j] != previewColor[i, j])
                    {
                        Console.SetCursorPosition(i, j);
                        Console.ForegroundColor = (ConsoleColor)(color[i, j] % 16);
                        Console.BackgroundColor = (ConsoleColor)(color[i, j] / 16);
                        Console.Write(drawBuffer[i, j]);
                        previewBuffer[i, j] = drawBuffer[i, j];
                        previewColor[i, j] = color[i, j];
                    }
                    drawBuffer[i, j] = Game.Background.character;
                    color[i, j] = (byte)((byte)Game.Background.color + (byte)Game.Background.bgColor * 16);
                }
            }
        }
    }
}
