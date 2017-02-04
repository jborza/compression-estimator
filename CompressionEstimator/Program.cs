using LibCompressionEstimator;
using System;

namespace CompressionEstimator
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = args[0];
            //EstimateDirectory(directory);
            MakeHtmlReport(directory);
        }

        private static void MakeHtmlReport(string directory)
        {
            var est = new DirectoryEstimator(new ConsoleLogger(), skipCompressedDirectories: true, parallelRun: true);
            Console.WriteLine($"Estimating {directory} with a block size of {est.BLOCK_SIZE} and maximum input size of {est.MAX_READ_SIZE} bytes");
            var results = est.Estimate(directory);
            //make json
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(results);
            Console.WriteLine(json);
        }

        private static void EstimateDirectory(string directory)
        {
            var est = new DirectoryEstimator(new ConsoleLogger(), skipCompressedDirectories: true, parallelRun: true);
            Console.WriteLine($"Estimating {directory} with a block size of {est.BLOCK_SIZE} and maximum input size of {est.MAX_READ_SIZE} bytes");
            var results = est.Estimate(directory);
            foreach (var result in results)
            {
                Console.WriteLine(result.ToString());
            }
        }

        private class ConsoleLogger : ILogger
        {
            public void Log(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
}
