using System;
using System.Collections.Generic;
using System.IO;

namespace LibCompressionEstimator
{
    interface IEstimatorRunner
    {
        IEnumerable<EstimationResult> Estimate(IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, EstimationResult> estimateFunc);
    }
}
