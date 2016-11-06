using LibCompressionEstimator.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator
{
    class SkipCompressedDirectoryEnumerator : IDirectoryEnumerator
    {
        public IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo parent)
        {
            return parent.EnumerateDirectories().Where(d => d.IsCompressed() == false);
        }
    }
}
