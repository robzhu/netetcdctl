using System;

namespace EtcdCtl
{
    public static class ConsoleEx
    {
        public static void ClearAndWriteSuccess( string value )
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine( value );
        }

        public static void ClearAndWriteError( string value )
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( value );
        }
    }
}
