using System.IO;

namespace BikeMgr.Core.Models
{
    public class Blob
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Stream FileStream { get; set; }
    }
}
