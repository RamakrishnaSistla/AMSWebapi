using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AssetManagementStoreApp
{
    public class AssetInfo
    {
        [Key]
        public int AssetId { get; set; }
        [MaxLength(500)]
        public string TechnologyUsed { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string UploadedBy { get; set; } = string.Empty;
        [Required]
        [MaxLength(100000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime UploadedOn { get; set; }
        [Required]
        [MaxLength(100000)]
        public string Comments { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int IdeaId { get; set; }
        [NotMapped]
        public string categoryName { get; set; } = string.Empty;
        [NotMapped]
        public string IdeaName { get; set; } = string.Empty;
        [NotMapped]
        public Idea? ObjIdea { get; set; }
        [NotMapped]
        public List<FileData>? Files { get; set; }
        [NotMapped]
        public List<Category>? Categories { get; set; }
    }
}