using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoAndHash
{
    public static class ConsoleHelper
    {
        public static ConsoleColor HighlightColor { get; set; } = ConsoleColor.Yellow;
        public static ConsoleColor HighlightBackgroundColor { get; set; } = ConsoleColor.Black;
        //
        public static ConsoleColor SuccessColor { get; set; } = ConsoleColor.Green;
        public static ConsoleColor SuccessBackgroundColor { get; set; } = ConsoleColor.Black;

        public static void WriteHightlight(this object obj)
        {
            // Storing the state
            var previousHighlight = Console.ForegroundColor;
            var previousBackHighlight = Console.BackgroundColor;
            // Changing the color
            Console.ForegroundColor = HighlightColor;
            Console.BackgroundColor = HighlightBackgroundColor;
            // Writing the object
            Console.Write(obj);
            // Restoring the state
            Console.ForegroundColor = previousHighlight;
            Console.BackgroundColor = previousBackHighlight;
        }

        public static void WriteSuccess(this object obj)
        {
            // Storing the state
            var previousHighlight = Console.ForegroundColor;
            var previousBackHighlight = Console.BackgroundColor;
            // Changing the color
            Console.ForegroundColor = SuccessColor;
            Console.BackgroundColor = SuccessBackgroundColor;
            // Writing the object
            Console.Write(obj);
            // Restoring the state
            Console.ForegroundColor = previousHighlight;
            Console.BackgroundColor = previousBackHighlight;
        }
    }
}
