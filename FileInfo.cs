using System.Collections.Generic;

namespace Tag
{
    public class FileInfo
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public override bool Equals(object o)
        {
            if(o == null)
                return false;

            if(!(o is FileInfo))
                return false;
            return this.FileName == (o as FileInfo).FileName && this.FileSize == (o as FileInfo).FileSize;
        }

        public override int GetHashCode()
        {
            return FileSize.GetHashCode() ^ FileName.GetHashCode();
        }
    }
}