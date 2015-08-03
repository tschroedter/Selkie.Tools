using System;
using System.Linq;
using System.Reflection;

namespace IsNUnitDll
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

            var isNunitDll = IsNUnitTestAssembly(testDll);

            Console.WriteLine(isNunitDll ? "Yes" : "No");
        }

        public static bool IsNUnitTestAssembly(Assembly testDll)
        {
            foreach (var type in testDll.GetTypes())
            {
                var attributes = type.GetCustomAttributes(true);

                var hasAttribute = attributes.Any(x => x.GetType().FullName.Contains("TestFixtureAttribute"));

                if (hasAttribute)
                {
                    return true;
                }
            }

            return false;
        }
    }
}