using System.IO;
using System.Threading.Tasks;

namespace BikeMgr.Core.Interface.Services
{
    public interface IStorageService
    {
        Task<string> SaveFile(Stream file, string savePath);
        void DeleteFile(string filePath);
    }
}
