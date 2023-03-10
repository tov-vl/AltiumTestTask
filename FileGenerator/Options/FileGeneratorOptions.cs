namespace FileGenerator.Options
{
    public static class FileGeneratorOptions
    {
        public static string FileName { get; set; } = "data.txt";

        public static string Directory { get; set; } = Environment.CurrentDirectory;
    }
}
