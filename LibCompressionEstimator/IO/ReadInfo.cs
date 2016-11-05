using System.IO;

namespace LibCompressionEstimator.IO
{
    internal class ReadInfo
    {
        public readonly long StartOffset;
        public readonly int BytesToRead;
        public readonly FileInfo File;

        public ReadInfo(FileInfo file, long startOffset, int bytesToRead)
        {
            this.File = file;
            this.StartOffset = startOffset;
            this.BytesToRead = bytesToRead;
        }
    }
}
