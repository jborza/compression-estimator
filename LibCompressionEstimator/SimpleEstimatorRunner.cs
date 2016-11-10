using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator
{
    class SimpleEstimatorRunner : IEstimatorRunner
    {
        public IEnumerable<EstimationResult> Estimate(IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, EstimationResult> estimateFunc)
        {
            return directories.Select(estimateFunc);
        }
    }
}
