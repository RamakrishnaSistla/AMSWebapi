using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AssetManagementStoreApp
{
    public class FileData
    {
        [Key]
        public int FileId { get; set; }
        [Required]
        [MaxLength(200)]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public byte[]? FileContent { get; set; }
        [Required]
        [MaxLength(200)]
        public string FileType {get;set;} = string.Empty;
        [Required]
        public DateTime dateTime {get;set;}
        public bool IsUploaded {get;set;}
        public int AssetId { get; set; }
        [NotMapped]
        public bool IsReplaced { get; set; }
    }
}