using System;
using System.Collections.Generic;
using System.IO;
//using System.Runtime.InteropServices;
using System.Text;

namespace BackupSplitterCD
{
    class Program
    {
        private const int MB = 1000000;
        private static string _folder;
        private static string _destFolder;
        private static bool _sameDestination;
        private static long _chunkSize;
        private static bool _move;
        private static bool _owerride;

        private static List<string> _skippedFiles = new List<string>();
        private static long _totalCount;
        private static long _totalSize;
        private static string[] _files;

        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern IntPtr GetStdHandle(int nStdHandle);

        //[DllImport("kernel32.dll")]
        //static extern bool ReadConsoleW(IntPtr hConsoleInput, [Out] byte[]
        //   lpBuffer, uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead,
        //   IntPtr lpReserved);

        //public static IntPtr GetWin32InputHandle()
        //{
        //    const int STD_INPUT_HANDLE = -10;
        //    IntPtr inHandle = GetStdHandle(STD_INPUT_HANDLE);
        //    return inHandle;
        //}


        static void Main(string[] args)
        {
            Console.WriteLine("Folter to backup:");
            _folder = Console.ReadLine();

            if(!Directory.Exists(_folder))
            {
                Console.WriteLine("NOT FOUND");
                return;
            }

            Console.WriteLine("Same destination?");
            string yesNo = Console.ReadLine();
            _sameDestination = yesNo.ToLower() == "yes";

            if (!_sameDestination)
            {
                Console.WriteLine("Destination folter:");
                _destFolder = Console.ReadLine();

                if (!Directory.Exists(_destFolder))
                {
                    Console.WriteLine("NOT FOUND");
                    return;
                }
            }
            else
            {
                _destFolder = _folder;
            }

            Console.WriteLine("Size of CD MB:");
            _chunkSize = Convert.ToInt64(Console.ReadLine()) * MB;

            Console.WriteLine("Move files?");
            _move = Console.ReadLine().ToLower() == "yes";

            Console.WriteLine("Owerride partial results?");
            _owerride = Console.ReadLine().ToLower() == "yes";

            ScanForFilesLargeWhenSize();
            PreStatistics();

            Console.WriteLine("Are you sure tu start?");
            yesNo = Console.ReadLine();

            if(yesNo.ToLower() != "yes")
            {
                Console.WriteLine("FINISED");
                return;
            }

            ProcessFolter();
        }

        //private static string ConsoleReadLine()
        //{
        //    const int bufferSize = 1024;
        //    var buffer = new byte[bufferSize];
        //
        // uint charsRead = 0;
        //
        //  ReadConsoleW(GetWin32InputHandle(), buffer, bufferSize, out charsRead, (IntPtr)0);
        // -2 to remove ending \n\r
        //  int nc = ((int)charsRead - 2) * 2;
         //   var b = new byte[nc];
         //   for (var i = 0; i < nc; i++)
         //       b[i] = buffer[i];

         //   var utf8enc = Encoding.UTF8;
         //   var unicodeenc = Encoding.Unicode;
         //   return utf8enc.GetString(Encoding.Convert(unicodeenc, utf8enc, b));
        //}

        private static void ProcessFolter()
        {
            int chunkIndex = 0;
            long currentSize = 0;
            string currentFolter = string.Empty;
            long currentCount = 0;
            foreach (var file in _files)
            {
                string chunkName = string.Empty;
                var info = new FileInfo(file);
                if (chunkIndex == 0 || currentSize + info.Length > _chunkSize)
                {
                    if (chunkIndex > 0)
                    {
                        Console.WriteLine("Chunk created: {0}", currentFolter);
                        Console.WriteLine("Chunk size: {0} MB", currentSize / MB);
                        Console.WriteLine("Chunk file count: {0}", currentCount);
                    }
                    currentSize = 0;
                    currentCount = 0;
                    chunkIndex++;
                    chunkName = $"CD_{chunkIndex}";
                    currentFolter = Path.Combine(_destFolder, chunkName);
                    Directory.CreateDirectory(currentFolter);
                }

                string subFolder = CreateChunkSubfolder(file, currentFolter);
                string destFile = Path.Combine(currentFolter, subFolder, info.Name);
                long size = info.Length;
                if (File.Exists(destFile) && _owerride)
                {
                    File.Delete(destFile);
                }
                if (!File.Exists(destFile))
                {
                    if (_move)
                    {
                        File.Move(file, destFile);
                    }
                    else
                    {
                        File.Copy(file, destFile);
                    }
                    currentSize += size;
                    currentCount++;
                }
                else
                {
                    currentSize += new FileInfo(destFile).Length;
                }
            }
        }

        private static string CreateChunkSubfolder(string file, string destFolder)
        {
            string result = Path.GetDirectoryName(file).Replace(_folder, string.Empty).TrimStart(Path.DirectorySeparatorChar);
            string[] subfolders = result.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            string curentFolder = destFolder;
            foreach(var folder in subfolders)
            {
                curentFolder = Path.Combine(curentFolder, folder);
                if (!Directory.Exists(curentFolder))
                {
                    Directory.CreateDirectory(curentFolder);
                }
            }
            return result;
        }

        private static void PreStatistics()
        {
            Console.WriteLine("====================");
            Console.WriteLine("Total size to process: {0} MB", _totalSize / MB);
            Console.WriteLine("Total files count to process: {0}", _totalCount);
            Console.WriteLine("Minimal estimated chunks: {0}", (_totalSize / _chunkSize)+1);
            Console.WriteLine("Destination folder: {0}", _destFolder);
            Console.WriteLine("====================");
        }

        private static void ScanForFilesLargeWhenSize()
        {
            _files = Directory.GetFiles(_folder, "*", SearchOption.AllDirectories);
            foreach (var file in _files)
            {
                var info = new FileInfo(file);
                if (info.Length > _chunkSize)
                {
                    _skippedFiles.Add(file);
                    Console.WriteLine("{0} SKIPPED with size {1} MB", file, info.Length / MB);
                }
                else
                {
                    _totalCount++;
                    _totalSize += info.Length;
                }
            }
        }
    }
}
