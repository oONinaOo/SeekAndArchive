using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SeekAndArchive
{
    class Program
    {
        static void Main(string[] args)
        {
            Method method = new Method();
            string directoryName = args[0];
            string fileName = args[1];
            string separator = "\\";

            string file = @"" + directoryName + separator + fileName;
            FileInfo fileinfo = new FileInfo(file);

            string filePath = method.searchFile(fileinfo);

            DirectoryInfo directorySelected = new DirectoryInfo(directoryName);
            
            string originalHash = System.Text.Encoding.UTF8.GetString(method.GetFileHash(filePath));

            // see if the file was ever compressed if not create it
            if (File.Exists(file + ".gz"))
            {
                foreach (FileInfo fileToDecompress in directorySelected.GetFiles(fileName + ".gz"))
                {
                    
                    string[] getFile = filePath.Split('.');
                    string compressed = getFile[0] + "decompressed." + getFile[1];
                    method.Decompress(fileToDecompress, compressed);
                    string compressedHash = System.Text.Encoding.UTF8.GetString(method.GetFileHash(compressed));
                    File.Delete(compressed);

                    //compare the last compress file's hashcode to original file
                    if (compressedHash == originalHash)
                    {
                        Console.WriteLine("Original file wasn't changed");
                    }
                    else
                    {
                        Console.WriteLine("Original file was changed");
                        method.Compress(directorySelected, fileinfo, directoryName, separator);
                    }
                }

            } else
            {
                method.Compress(directorySelected, fileinfo, directoryName, separator);
            }
            

        }
        
    }
}
