using System.IO;

namespace LibCompressionEstimator.IO
{
    public static class DirectoryInfoExtensions
    {
        public static bool IsCompressed(this DirectoryInfo di)
        {
            return (di.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
        }
    }
}
