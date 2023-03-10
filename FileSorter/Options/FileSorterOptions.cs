using FileSorter.Model;

namespace FileSorter.Options
{
    public static class FileSorterOptions
    {
        public static string Directory { get; set; } = FileOutputOptions.Directory + @"\temp";

        public static IComparer<FileRow> FileRowComparer { get; set; } = new FileRowComparer();

        public static long BytesPerChunk { get; set; } = 1 * 1024 * 1024 * 1024; //2 * 1024 * 1024;

        public static int MergesPerCycle { get; set; } = 10;
    }
}