using System.ComponentModel.DataAnnotations;
namespace AssetManagementStoreApp
{
    public class Idea
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100000)] 
        public string Description {get; set;} = string.Empty;
    }
}