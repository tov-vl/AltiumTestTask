using System.Diagnostics;

namespace FileGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Started file generating...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Generator.GenerateFile(args);

            var elapsedTime = stopwatch.Elapsed;
            Console.WriteLine($"Finished generating! Time elapsed: {elapsedTime.TotalSeconds:0.###}s");
            Console.WriteLine("Press any key to finish...");
            Console.ReadKey();
        }
    }
}