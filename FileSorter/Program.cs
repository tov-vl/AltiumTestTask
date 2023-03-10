using FileGenerator.Options;
using FileSorter.Options;
using System.Diagnostics;

namespace FileSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Started sorting...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Sorter.SortFile(
                Path.Combine(FileGeneratorOptions.Directory, FileGeneratorOptions.FileName),
                Path.Combine(FileOutputOptions.Directory, FileOutputOptions.FileName));

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            Console.WriteLine($"Finished sorting! Time elapsed: {elapsedTime.TotalSeconds:0.###}s");
            Console.WriteLine("Press any key to finish...");
            Console.ReadKey();
        }
    }
}