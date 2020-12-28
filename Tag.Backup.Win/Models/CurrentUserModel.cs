using System.Collections.Generic;
using Tag.Common.Models;

namespace Tag.Backup.Win.Models
{
    public class CurrentUserModel
    {
        public User User { get; set; }
        public List<string> BackupDirectories { get; set; } = new List<string>();
    }
}