using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateHandler
{
    /*
     This ArchiveFileContext interface is used to abstract over the ArchiveFileContext implementation,
     allowing us to parse an IArchiveFileContext as the parameter to functions as well as to use it in
     unit tests, where we use an in memory sqlite database.
     */
    public interface IArchiveFileContext
    {

        DbSet<ArchiveFile> Files { get; set; }
        DbSet<ConvertedFile> _ConvertedFiles { get; set; }
    }
}
