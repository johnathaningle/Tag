using System.Collections.Generic;

namespace Tag
{
    public class FileInfo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}