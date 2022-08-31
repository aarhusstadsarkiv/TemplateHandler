using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateHandler;
namespace TemplateHandlerTests
{

    /*
        The ArchiveFile context used in unit tests.
        Note that it has another constructor as it uses an in memory database
        which is created and destroyed in the testclass.
     */
    public class TestArchiveFileContext : DbContext, IArchiveFileContext
    {
        public DbSet<ArchiveFile> Files { get; set; }

        public TestArchiveFileContext(DbContextOptions<TestArchiveFileContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite(
                @"Server=Data Source={DbPath}");
        }

    }
}
