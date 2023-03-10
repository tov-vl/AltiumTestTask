using FileGenerator.Options;
using FileSorter.Model;

namespace FileGenerator
{
    public class Generator
    {
        private const int MinLines = 2_000_000;
        private const int MaxLines = 2_000_000;

        public static void GenerateFile(string[] args)
        {
            int linesNum;
            string? strContent = null;

            if (args.Length == 0)
            {
                var randGen = new Random();
                linesNum = randGen.Next(MinLines, MaxLines);
            }
            else
            {
                linesNum = int.Parse(args[0]);
                strContent = args[1];
            }

            WriteToFile(linesNum, strContent);
        }

        private static void WriteToFile(int linesNum, string? strContent)
        {
            string path = Path.Combine(FileGeneratorOptions.Directory, FileGeneratorOptions.FileName);
            using TextWriter writer = File.CreateText(path);

            var randGen = new Random();
            var counter = 0;

            var randomStringContent = strContent == null;
            while (counter < linesNum)
            {
                var num = randGen.Next();

                if (randomStringContent)
                {
                    var strLength = randGen.Next(14, 16);
                    var strCounter = 0;
                    strContent = string.Empty;
                    while (strCounter < strLength)
                    {
                        strContent += (char)randGen.Next(65, 91);
                        strCounter++;
                    }
                }

                var row = new FileRow { Number = num, StringContent = strContent };

                writer.WriteLine(row);

                counter++;
            }
        }
    }
}