using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example_1
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                Process p = Factory.Make();

                p.MoveAndToggleAsync(1).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }

        static class Factory
        {
            public static Process Make()
            {
                return new Process();
            }
        }
    }
}
