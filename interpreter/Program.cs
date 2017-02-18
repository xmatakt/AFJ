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
            try
            {
                //dell
                //var prg =
                //new NLProgram(@"C:\AATimo\tmp\increment.txt",
                //    @"C:\AATimo\tmp\input.bin", @"C:\AATimo\tmp\output.bin");

                //var prg = new NLProgram(@"C:\AATimo\tmp\writeA.txt", @"C:\AATimo\tmp\input.bin",
                //    @"C:\AATimo\tmp\output.bin");

                //var prg = new NLProgram(@"C:\AATimo\tmp\prg.txt", @"C:\AATimo\tmp\input.bin",
                //    @"C:\AATimo\tmp\output.bin");

                //var prg = new NLProgram(@"C:\AATimo\tmp\writeA_decadic.txt", @"C:\AATimo\tmp\input.bin",
                //   @"C:\AATimo\tmp\output.bin");

                //var prg = new NLProgram(@"C:\AATimo\tmp\readAddWrite.txt", @"C:\AATimo\tmp\input.bin",
                //   @"C:\AATimo\tmp\output.bin");

                //var prg =
                //    new NLProgram(@"C:\AATimo\tmp\helloWorld.txt",
                //        @"C:\AATimo\tmp\input.bin", @"C:\AATimo\tmp\output.bin");

                //  dell

                //  asus
                //var prg = new NLProgram(@"C:\AATimo\prg.txt", "input.bin", "output.bin");
                //var prg = new NLProgram(@"C:\AATimo\writeA1.txt", "input.bin", "output.bin");
                //  asus

                var prg = new NLProgram(args[0], args[1], args[2]);

                if (prg.ExecuteProgram() == 0)
                    Console.WriteLine("\nProgram exited with status code 0");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError in program execution:");
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nStack trace:");
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPress any key to escape...");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ReadLine();
        }
    }
}
