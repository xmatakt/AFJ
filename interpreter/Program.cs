using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //new NLProgram(@"C:\AATimo\Programy\API\AJF\zadanie_1\interpreter\interpreter\bin\Debug\prg.txt", "input.bin", "output.bin");
            //new NLProgram(@"C:\AATimo\Programy\API\AJF\zadanie_1\interpreter\interpreter\bin\Debug\increment.txt", "input.bin", "output.bin");
            //new NLProgram(@"C:\AATimo\Programy\API\AJF\zadanie_1\interpreter\interpreter\bin\Debug\writeA.txt", "input.bin", "output.bin");

            var prg = new NLProgram(@"C:\AATimo\prg.txt", "input.bin", "output.bin");
            //var prg = new NLProgram(@"C:\AATimo\writeA1.txt", "input.bin", "output.bin");
            try
            {
                prg.ExecuteProgram();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            

            Console.ReadLine();
        }
    }
}
