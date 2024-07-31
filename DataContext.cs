global using Microsoft.EntityFrameworkCore;
namespace AssetManagementStoreApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<FileData> Files {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<AssetInfo> Assets {get;set;}
        public DbSet<Idea> Ideas {get;set;}
    }
}