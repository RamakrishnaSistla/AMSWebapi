using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AssetManagementStoreApp
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName {get; set;} = string.Empty;
        [NotMapped]
        public List<Idea>? Ideas {get;set;}
    }
}