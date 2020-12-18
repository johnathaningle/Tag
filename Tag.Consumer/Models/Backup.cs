using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tag.Consumer.Models
{
    public class Backup
    {
        [Key]
        public int BackupId { get; set; }
        public string Name { get; set; }
        [Column("Schema")]
        private string schema { get; set; }
        [NotMapped]
        public FileStructure Schema
        {
            get
            {
                if(string.IsNullOrEmpty(this.schema))
                    return new FileStructure();

                return JsonSerializer.Deserialize<FileStructure>(this.schema);
            }
            set
            {
                this.schema = JsonSerializer.Serialize(schema);
            }
        }
    }
}