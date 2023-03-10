namespace FileSorter.Model
{
    [Serializable]
    public class FileRow
    {
        public int Number { get; set; }

        public string? StringContent { get; set; }

        public override string ToString()
        {
            return $"{Number}. {StringContent}";
        }
    }
}
