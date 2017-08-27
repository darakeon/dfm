using System;

namespace Test
{
    internal class Base
    {
        protected T SafeExecutor<T>(Func<T> action)
        {
            Console.WriteLine("...");

            try
            {
                var result = action();

                Console.WriteLine("=)");

                return result;
            }
            catch (Exception)
            {
                Console.WriteLine("Oh shit!");
                throw;
            }
        }

        protected void SafeExecutor(Action action)
        {
            Console.WriteLine("...");

            try
            {
                action();

                Console.WriteLine("=)");
            }
            catch (Exception)
            {
                Console.WriteLine("Oh shit!");
                throw;
            }
        }

    }
}