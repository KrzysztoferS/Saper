using System;

namespace Saper.Intrfaces
{
    class Display : IDisplay
    {
        public void DisplayOnScreen(int row, int col, string text)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }
}
