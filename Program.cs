using System;
using System.Threading;

namespace Template
{
    static class Program
    {
        //Mutex for securing thread enforcement
        private static Mutex thereCanBeOnlyOne;

        static void Main()
        {
            const string mutexName = "uniqueMutex"; //Defines a unique name for the Mutex
            bool isNewInstance;
            thereCanBeOnlyOne = new Mutex(true, mutexName, out isNewInstance); //returns false if there's another Mutex with the same name, thus not running the program
            if (!isNewInstance)
            {
                return;
            }
            using var game = new GameWorld();
            game.Run();

            GC.KeepAlive(thereCanBeOnlyOne); //Prevents GC (Garbage Collector) from removing Mutex untimely
        }
    }

}