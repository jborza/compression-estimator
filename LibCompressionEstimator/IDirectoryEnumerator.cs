using System.Collections.Generic;
using System.IO;

namespace LibCompressionEstimator
{
    internal interface IDirectoryEnumerator
    {
        IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo parent);
    }
}
