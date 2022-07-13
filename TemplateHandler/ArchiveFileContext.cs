using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace TemplateHandler
{
    internal class ArchiveFileContext : DbContext
    {
        public DbSet<ArchiveFile> Files { get; set; }
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
