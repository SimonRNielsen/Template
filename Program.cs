using System;
using System.Threading;

namespace Template
{
    static class Program
    {

        private static Mutex thereCanBeOnlyOne;

        static void Main()
        {
            const string mutexName = "uniqueMutex";
            bool isNewInstance;
            thereCanBeOnlyOne = new Mutex(true, mutexName, out isNewInstance);
            if (!isNewInstance)
            {
                return;
            }
            using var game = new GameWorld();
            game.Run();

            GC.KeepAlive(thereCanBeOnlyOne);
        }
    }

}