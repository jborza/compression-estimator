using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator.IO
{
    internal class DirectoryFilesManager
    {       
        private List<DirectoryRecord> records;

        public DirectoryFilesManager(IEnumerable<FileInfo> files)
        {
            this.Length = files.Sum(p => p.Length);
            records = PrepareRecords(files).ToList();
        }

        private IEnumerable<DirectoryRecord> PrepareRecords(IEnumerable<FileInfo> files)
        {
            long currentFileStart = 0;
            foreach (var file in files)
            {
                yield return new DirectoryRecord(file, start: currentFileStart, end: currentFileStart + file.Length);
                currentFileStart += file.Length;
            }
        }

        public readonly long Length;

        internal IEnumerable<ReadInfo> GetFilesToRead(long position, int count)
        {
            long endPosition = position + count;
            var filesToConsider = from r in records
                                  where r.Start < endPosition && r.End > position
                                  select r;
            long currentOffset = position;
            foreach (var f in filesToConsider)
            {
                var globalStartOffset = Math.Max(currentOffset, f.Start);
                var globalEndOffset = Math.Min(endPosition, f.End);
                var bytesToRead = globalEndOffset - globalStartOffset;
                var fileStartOffset = globalStartOffset - f.Start;
                yield return new ReadInfo(f.File, fileStartOffset, (int)bytesToRead);
                currentOffset += bytesToRead;
            }
        }

        private class DirectoryRecord
        {           
            public readonly FileInfo File;
            public readonly long Start;
            public readonly long End;

            public DirectoryRecord(FileInfo file, long start, long end)
            {
                this.File = file;
                this.Start = start;
                this.End = end;
            }
        }
    }
}
