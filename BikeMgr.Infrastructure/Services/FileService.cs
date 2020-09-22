using BikeMgr.Core.Interface.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BikeMgr.Infrastructure.Services
{
    public class FileService : IStorageService
    {
        private Options _options;

        public FileService(Options options)
        {
            _options = options;
        }
        
        public async Task<string> SaveFile(Stream file, string savePath)
        {
            if (file == null) throw new Exception("Invalid file.");
            if (String.IsNullOrEmpty(savePath)) throw new Exception("Invalid file path.");
            //Security Risk --file uploads must be virus scanned            
            //try
            //{
            //    var absPath = Path.Combine(_options.RootPath, savePath);
            //    using (var outputStream = File.OpenWrite(absPath))
            //    {
            //        await file.CopyToAsync(outputStream);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Could not save file.", ex);
            //    throw;
            //}
            //return savePath;
            return "";
        }

        public void DeleteFile(string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return;
            try
            {
                var absPath = Path.Combine(_options.RootPath, filePath);
                File.Delete(absPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete file.", ex);
            }
        }

        public class Options
        {
            public string RootPath { get; set; }
        }
    }
}
