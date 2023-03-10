using FileGenerator.Options;
using FileSorter.Model;
using System.Diagnostics;

namespace FileGenerator
{
    public class Generator
    {
        private const int MinLines = 2_000_000;
        private const int MaxLines = 2_000_000;

        public static void GenerateFile(string[] args)
        {
            int linesNum;
            string strContent;

            if (args.Length == 0)
            {
                var randGen = new Random();

                linesNum = randGen.Next(MinLines, MaxLines);

                var strLength = randGen.Next(1, 15);
                var counter = 0;
                strContent = string.Empty;
                while (counter < strLength)
                {
                    strContent += (char)randGen.Next(65, 90);
                    counter++;
                }
            }
            else
            {
                linesNum = int.Parse(args[0]);
                strContent = args[1];
            }

            WriteToFile(linesNum, strContent);
        }

        private static void WriteToFile(int linesNum, string strContent)
        {
            string path = Path.Combine(FileGeneratorOptions.Directory, FileGeneratorOptions.FileName);
            using TextWriter writer = File.CreateText(path);

            var randGen = new Random();
            var counter = 0;
            while (counter < linesNum)
            {
                var num = randGen.Next();

                var strLength = randGen.Next(14, 16);
                var strCounter = 0;
                strContent = string.Empty;
                while (strCounter < strLength)
                {
                    strContent += (char)randGen.Next(65, 91);
                    strCounter++;
                }

                var row = new FileRow { Number = num, StringContent = strContent };

                writer.WriteLine(row);

                counter++;
            }
        }
    }
}