using LibCompressionEstimator.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator
{
    public class DirectoryEstimator
    {
        /// <summary>
        /// Suggest that a directory should be compressed when its compression ratio is below or equal 
        /// to the threshold
        /// </summary>
        public double Threshold = 0.75;
        public int MAX_READ_SIZE = 1024 * 1024 * 50;
        public int BLOCK_SIZE = 1024 * 512;

        public DirectoryEstimator()
        {

        }

        public IEnumerable<EstimationResult> Estimate(string directory)
        {
            var di = new DirectoryInfo(directory);
            return di.EnumerateDirectories().Select(EstimateDirectory);
        }

        private EstimationResult EstimateDirectory(DirectoryInfo arg)
        {
            DirectoryStream ds = new DirectoryStream(arg.FullName);
            StreamEstimator se = new StreamEstimator();
            var packedSize = se.EstimatePackedSize(ds, MAX_READ_SIZE, BLOCK_SIZE);
            return new EstimationResult()
            {
                Directory = arg.FullName,
                ShortName = arg.Name,
                OriginalSize = ds.Length,
                EstimatedSize = packedSize
            };
        }
    }
}
