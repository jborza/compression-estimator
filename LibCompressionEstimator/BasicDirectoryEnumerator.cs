using System.Collections.Generic;
using System.IO;

namespace LibCompressionEstimator
{
    class BasicDirectoryEnumerator : IDirectoryEnumerator
    {
        public IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo parent)
        {
            return parent.EnumerateDirectories();
        }
    }
}
