using System;

namespace TeoEngine
{
    public class Visibility
    {
        public char character = '#';
        public ConsoleColor color = ConsoleColor.White;
        public ConsoleColor bgColor = ConsoleColor.Black;
        public bool transparentColor = false;
        public bool transparentBgColor = false;
    }

    public enum ObjectSpace
    {
        Screen,
        World
    }

    public enum VerticalAlign
    {
        Up,
        Center,
        Down
    }

    public enum HorizontalAlign
    {
        Left,
        Center,
        Right
    }
}
