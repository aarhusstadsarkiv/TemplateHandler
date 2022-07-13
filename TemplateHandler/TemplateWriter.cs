using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateHandler
{
    internal class TemplateWriter
    {
        

        public static void InsertTemplate(object file, string destinationRoot, byte [] template_content)
        {
            // Cast the file to an ArchiveFile
            ArchiveFile archiveFile = (ArchiveFile)file;

            // Get the output directory.
            string outdir = Path.Combine(destinationRoot, archiveFile.RelativePath);
            outdir = Directory.GetParent(outdir).FullName;
            
            // Create the output directory
            Directory.CreateDirectory(outdir);
            string outputFile = Path.Combine(outdir, "1.tif");
            
            File.WriteAllBytes(outputFile, template_content);
        }
    }
}
