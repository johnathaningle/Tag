using System.Collections.Generic;

namespace Tag.Common.Models
{
    public class FileStructure
    {
        public string FolderName { get; set; }
        public List<FileStructure> SubFolders { get; set; } = new List<FileStructure>();
    }
}