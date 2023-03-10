using FileSorter.Model;

namespace FileSorter
{
    internal class FileRowComparer : IComparer<FileRow>
    {
        public int Compare(FileRow? x, FileRow? y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));

            if (y == null)
                throw new ArgumentNullException(nameof(y));

            var compareStringResult = Comparer<string>.Default.Compare(x.StringContent, y.StringContent);

            if (compareStringResult != 0)
            {
                return compareStringResult;
            }
            else
            {
                var compareNumResult = Comparer<int>.Default.Compare(x.Number, y.Number);
                return compareNumResult;
            }
        }
    }
}
