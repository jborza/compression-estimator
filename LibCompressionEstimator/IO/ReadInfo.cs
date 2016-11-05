using System.IO;

namespace LibCompressionEstimator.IO
{
    internal class ReadInfo
    {
        public long StartOffset;
        public int BytesToRead;
        public FileInfo File;
    }
}
