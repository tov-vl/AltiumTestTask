namespace FileSorter.Options
{
   public class FileOutputOptions
    {
        public static string FileName { get; set; } = "data_sorted.txt";

        public static string Directory { get; set; } = Environment.CurrentDirectory;
    }
}
