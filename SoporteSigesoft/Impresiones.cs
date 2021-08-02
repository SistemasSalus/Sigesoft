using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoporteSigesoft
{
    public static class Impresiones
    {
        public static void ImprimirLinea(ConsoleColor colorFondo, ConsoleColor colorFuente, string texto, bool alingCenter)
        {
            Console.BackgroundColor = colorFondo;
            Console.ForegroundColor = colorFuente;
            if (alingCenter)
                Console.SetCursorPosition((Console.WindowWidth - texto.Length) / 2, Console.CursorTop);

            Console.WriteLine(texto);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ImprimirLinea(string texto)
        {
            Console.WriteLine(texto);
        }

        public static void ImprimirTexto(string texto)
        {
            Console.Write(texto);
        }

        public static void ImprimirTexto(ConsoleColor colorFondo, ConsoleColor colorFuente, string texto, bool alingCenter)
        {
            Console.BackgroundColor = colorFondo;
            Console.ForegroundColor = colorFuente;
            if (alingCenter)
                Console.SetCursorPosition((Console.WindowWidth - texto.Length) / 2, Console.CursorTop);

            Console.Write(texto);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
