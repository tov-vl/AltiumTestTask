using FileSorter.Model;
using FileSorter.Options;

namespace FileSorter
{
    public static class Sorter
    {
        private static readonly string SortedChunkTemplate = "sorted_chunk";
        private static readonly string MergedChunkTemplate = "merged_chunk";
        private static readonly string TempFileExt = ".tmp";
        private static readonly int EstimatedBytesPerRow = 40;

        public static void SortFile(string sourceFilePath, string outputFilePath)
        {
            var sortedFiles = SplitAndSortChunks(sourceFilePath);

            MergeChunks(sortedFiles, outputFilePath);
        }

        private static string[] SplitAndSortChunks(string sourceFilePath)
        {
            var fileRowsBuffer = new List<FileRow>((int)(FileSorterOptions.BytesPerChunk / EstimatedBytesPerRow));
            var sortedFilePaths = new List<string>();

            using var sourceReader = new StreamReader(File.OpenRead(sourceFilePath));

            var sortedChunkCounter = 0;
            while (!sourceReader.EndOfStream)
            {
                var totalBytes = 0;
                while (totalBytes < FileSorterOptions.BytesPerChunk)
                {
                    if (sourceReader.EndOfStream)
                        break;

                    var row = sourceReader.ReadLine();
                    var fileRow = ParseFileRow(row);

                    totalBytes += System.Text.Encoding.Unicode.GetByteCount(row!);
                    fileRowsBuffer.Add(fileRow!);
                }

                var sortedFilePath = Path.Combine(FileSorterOptions.Directory, $"{SortedChunkTemplate}_{++sortedChunkCounter}{TempFileExt}");

                if (!Directory.Exists(FileSorterOptions.Directory))
                    Directory.CreateDirectory(FileSorterOptions.Directory);

                SortBuffer(fileRowsBuffer);
                using var sortedFileWriter = new StreamWriter(File.Create(sortedFilePath));

                foreach (var fileRow in fileRowsBuffer)
                    sortedFileWriter.WriteLine(fileRow);

                fileRowsBuffer.Clear();
                sortedFilePaths.Add(sortedFilePath);
            }

            return sortedFilePaths.ToArray();

            static void SortBuffer(List<FileRow> buffer)
            {
                buffer.Sort(FileSorterOptions.FileRowComparer);
            }
        }

        private static void MergeChunks(string[] sortedFilePaths, string outputFilePath)
        {
            PreCleanUp();

            var mergedChunkCounter = 0;
            while (sortedFilePaths.Length > 1)
            {
                var filePathsChunks = sortedFilePaths.Chunk(FileSorterOptions.MergesPerCycle);

                foreach (var filePathsChunk in filePathsChunks)
                {
                    var mergedChunkFileName = $"{MergedChunkTemplate}_{++mergedChunkCounter}{TempFileExt}";
                    var mergedChunkFilePath = Path.Combine(FileSorterOptions.Directory, Path.GetFileName(mergedChunkFileName));

                    if (filePathsChunk.Length == 1)
                    {
                        File.Move(filePathsChunk.First(), mergedChunkFilePath, true);
                    }
                    else
                    {
                        using var mergedChunkStreamWriter = new StreamWriter(File.OpenWrite(mergedChunkFilePath));
                        Merge(filePathsChunk, mergedChunkStreamWriter);
                    }
                }

                sortedFilePaths = Directory.GetFiles(FileSorterOptions.Directory, $"{MergedChunkTemplate}*");
            }

            File.Move(sortedFilePaths.First(), outputFilePath, true);

            PostCleanUp();

            static void PreCleanUp()
            {
                var garbageFilePaths = Directory.GetFiles(FileSorterOptions.Directory, $"{MergedChunkTemplate}*");

                foreach(var filePath in garbageFilePaths)
                    File.Delete(filePath);
            }

            static void PostCleanUp()
            {
                Directory.Delete(FileSorterOptions.Directory);
            }
        }

        private static void Merge(string[] filePaths, StreamWriter streamWriter)
        {
            var streamFirstRowTuples = Init(filePaths);
            var streams = streamFirstRowTuples.Select(x => x.StreamReader).ToArray();

            while (streamFirstRowTuples.Count > 0)
            {
                streamFirstRowTuples.Sort((elem1, elem2) => FileSorterOptions.FileRowComparer.Compare(elem1.FileRow, elem2.FileRow));

                var valueToWrite = streamFirstRowTuples.First().FileRow;
                var stream = streamFirstRowTuples.First().StreamReader;

                streamWriter.WriteLine(valueToWrite);

                if(stream.EndOfStream)
                {
                    streamFirstRowTuples.RemoveAt(0);
                    continue;
                }

                var newFileRow = ParseFileRow(stream.ReadLine());
                streamFirstRowTuples[0] = (streamFirstRowTuples[0].StreamReader!, newFileRow!);
            }

            Cleanup(streams, filePaths);

            static List<(StreamReader StreamReader, FileRow FileRow)> Init(string[] filePaths)
            {
                var res = new List<(StreamReader, FileRow)>(filePaths.Length);

                foreach (var filePath in filePaths)
                {
                    var streamReader = new StreamReader(File.OpenRead(filePath));
                    var value = ParseFileRow(streamReader.ReadLine());

                    res.Add((streamReader!, value!));
                }

                return res;
            }

            static void Cleanup(IEnumerable<StreamReader> streams, IEnumerable<string> filePaths)
            {
                foreach (var stream in streams)
                    stream.Dispose();

                foreach (var filePath in filePaths)
                    File.Delete(filePath);
            }
        }

        static FileRow? ParseFileRow(string? row)
        {
            if (row == null)
                return null;

            var dotIndex = row.IndexOf('.');
            var num = int.Parse(row[..dotIndex]);
            var stringContent = row[(dotIndex + 2)..];

            return new()
            {
                Number = num,
                StringContent = stringContent
            };
        }
    }
}