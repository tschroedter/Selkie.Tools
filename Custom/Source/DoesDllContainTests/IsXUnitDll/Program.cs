using System;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace IsXUnitDll
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No - No parameters provided!");
                return;
            }

            var testDll = Assembly.LoadFrom(args[0]);

            var isXunitDll = IsNUnitTestAssembly(testDll);

            if (isXunitDll)
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }
        }

        public static bool IsNUnitTestAssembly(Assembly testDll)
        {
            foreach (var type in testDll.GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    if (methodInfo.GetCustomAttributes(typeof (FactAttribute),
                        true)
                        .Length > 0)
                    {
                        return true;
                    }

                    if (methodInfo.GetCustomAttributes(typeof (TheoryAttribute),
                        true)
                        .Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}