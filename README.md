# compression-estimator
Small SSD? This tool will try to help which folders you should compress with NTFS compression

I initially wrote this tool to help me decide which folders from my Steam library to compress (using Windows built-in NTFS compression) 
so I can save some disk space.

To provide some kind of assessment, this tool treats each directory as a continuous stream of bytes, builds a representative sample and compresses it 
using weak compression, then extrapolates the result as if the whole directory was compressed and calculates the disk space to be gained. 

Example: A directory has 150 MB, our sample size is 50 MB, so we could say that every third 1 MB block is picked, put together and compressed. 
This works across the files in the directory, so in theory would work if it has thousands of tiny files or just a few of large files.



