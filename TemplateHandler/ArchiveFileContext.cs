using Microsoft.EntityFrameworkCore;


namespace TemplateHandler
{
    /*
        The following class represents a DbContext for a files.db file with a Files table
        where each row represents an ArchiveFile.
        These archivefiles are parsed in to the Files DbSet. 
     */
    public class ArchiveFileContext : DbContext, IArchiveFileContext
    {
        public DbSet<ArchiveFile> Files { get; set; }
        public DbSet<ConvertedFile> _ConvertedFiles { get; set; }
        
        public string DbPath { get; }

        public ArchiveFileContext(string dbPath)
        {
            DbPath = dbPath;
        }

        // The following configures EF to use Sqlite.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

    }

}
