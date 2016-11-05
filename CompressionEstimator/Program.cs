using LibCompressionEstimator;
using System;

namespace CompressionEstimator
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = args[0];
            EstimateDirectory(directory);
        }

        private static void EstimateDirectory(string directory)
        {
            var est = new DirectoryEstimator();
            Console.WriteLine($"Estimating {directory} with a block size of {est.BLOCK_SIZE} and maximum input size of {est.MAX_READ_SIZE} bytes");
            var results = est.Estimate(directory);
            foreach (var result in results)
            {
                Console.WriteLine(result.ToString());
            }
        }
    }
}
