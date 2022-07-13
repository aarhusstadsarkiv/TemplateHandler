using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateHandler
{
    public class TemplateWriter
    {
        

        public static string InsertTemplate(object file, string destinationRoot, byte [] template_content)
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
            return outputFile;
        }

        public static byte[] GetTemplateFile(int id)
        {
            string filePath;
            switch (id)
            {
                case 0:
                    filePath = "Images/file_damaged.tif";
                    break;
                case 1:
                    filePath = "Images/file_empty.tif";
                    break;
                case 2:
                    filePath = "Images/file_not_convertable.tif";
                    break;
                case 3:
                    filePath = "Images/file_not_preservable.tif";
                    break;
                case 4:
                    filePath = "Images/password_protected.tif";
                    break;
                default:
                    filePath = "NULL";
                    break;
            }

            try
            {
                byte[] templateContent = File.ReadAllBytes(filePath);
                return templateContent;
            }

            catch (FileNotFoundException)
            {
                throw new ArgumentException(String.Format("The template id {0} is invalid. It must be between 0 and 4", id);
            }
        }
    }
}
