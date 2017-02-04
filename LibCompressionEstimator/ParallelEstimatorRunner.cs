using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LibCompressionEstimator
{
    class ParallelEstimatorRunner : IEstimatorRunner
    {
        public IEnumerable<EstimationResult> Estimate(IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, EstimationResult> estimateFunc)
        {
            var bag = new ConcurrentBag<EstimationResult>();
            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
            var result = Parallel.ForEach(directories, parallelOptions, d =>
            {
                bag.Add(estimateFunc(d));
            });
            return bag;
        }
    }
}
