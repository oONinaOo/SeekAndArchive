using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeekAndArchive
{
    class Method
    {
        // get the file we want 
        public string searchFile(FileInfo fileinfo)
        {
            var fileList = new DirectoryInfo(fileinfo.DirectoryName).GetFiles(fileinfo.Name, SearchOption.AllDirectories);
            string searchedFileName = "";
            foreach (var getFile in fileList)
            {
                if (getFile.ToString() == fileinfo.Name.ToString())
                {
                    searchedFileName = getFile.FullName;
                }
            }
            return searchedFileName;
        }

        // compress the file 
        public void Compress(DirectoryInfo directorySelected, FileInfo fileToCompress, string directoryName, string separator)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                           CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);

                        }
                    }
                    FileInfo info = new FileInfo(directoryName + separator + fileToCompress.Name + ".gz");
                    Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                    fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
                }

            }

        }
        //decompress the file 
        public void Decompress(FileInfo fileToDecompress, string compressed)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(compressed))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        //get file hascode, so it can be compared
        public byte[] GetFileHash(string fileName)
        {
            HashAlgorithm sha1 = HashAlgorithm.Create();
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                return sha1.ComputeHash(stream);
        }
    }
}
