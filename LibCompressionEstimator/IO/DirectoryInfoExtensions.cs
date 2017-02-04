using System.IO;

namespace LibCompressionEstimator.IO
{
    public static class DirectoryInfoExtensions
    {
        public static bool IsCompressed(this DirectoryInfo di)
        {
            if (di.Attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                string targetDirectory = di.GetLinkTargetPath();
                var targetDi = new DirectoryInfo(targetDirectory);
                return targetDi.IsCompressed();
            }
            else
            {
                return (di.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
            }
        }

        public static string GetLinkTargetPath(this DirectoryInfo di)
        {
            const string PREFIX = @"\\?\";
            string targetName = NativeMethods.GetFinalPathName(di.FullName);
            if (targetName.StartsWith(PREFIX))
            {
                return targetName.Substring(PREFIX.Length);
            }
            else
            {
                return targetName;
            }
        }
    }
}
