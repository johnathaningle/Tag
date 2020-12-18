using System.ComponentModel.DataAnnotations;

namespace Tag.Consumer.Models
{
    public class Backup
    {
        [Key]
        public int BackupId { get; set; }        
        public string Name { get; set; }
        [Column(Name = "Schema")]
        private string schema { get; set; }
    }
}