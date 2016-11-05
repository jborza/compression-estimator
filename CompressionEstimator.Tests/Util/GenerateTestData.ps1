$rnd = New-Object System.Random
function CreateRandomFile
{
    param( [int]$size, [string]$name)
    $bytes = New-Object byte[] $size
    $rnd.NextBytes($bytes)
    [System.IO.File]::WriteAllBytes($name, $bytes)
}

CreateRandomFile -size 16384 -name "file1.bin"
CreateRandomFile -size 2500000 -name "file2.bin"
CreateRandomFile -size 100000 -name "file3.bin"