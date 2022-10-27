using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateHandler
{
    /*
       The ArchiveFile class based on the schema for the Files table in the database.
        Each row in the Files table will be parsed as an ArchiveFile object with the Entity Framework.
     */
    public class ArchiveFile
    {
        // The value just needs to be less than 64.
        private const int PUID_MAX_LENGTH =  28;
        
        [Column("id")]
        public int ID { get; set; }

        [Column("uuid")]
        public string UUID { get; set; }

        [Column("relative_path")]
        public string RelativePath { get; set; }

        [Column("checksum")]
        public string Checksum { get; set; }

        [Column("puid")]
        public string? Puid { get; set; }

        [Column("signature")]
        public string? Signature { get; set; }

        [Column("is_binary")]
        public int IsBinary { get; set; }

        [Column("file_size_in_bytes")]
        public long Filesize { get; set; }

        /*
         * A nullable value type T? represents all values of its underlying value type T and an additional null value. 
         * For example, you can assign any of the following three values to a bool? variable: true, false, or null
         */
        [Column("warning")]
        public string? Warning { get; set; }

        public ArchiveFile(int iD, string uUID, string relativePath, string checksum, string puid,
                            string signature, int isBinary, long filesize, string? warning)
        {
            ID = iD;
            UUID = uUID;
            RelativePath = relativePath;
            Checksum = checksum;
            Puid = puid;
            Signature = signature;
            IsBinary = isBinary;
            Filesize = filesize;
            Warning = warning;
        }


        private static string[] getChecksums(string filePath)
        {
            string[] checksums = File.ReadAllLines(filePath);
            for (int i = 0; i < checksums.Length; i++)
            {
                checksums[i] = checksums[i].Replace("\n", String.Empty);
            }
            return checksums;
        }

        public static List<ArchiveFile> GetArchiveFiles(string queryParameter, IArchiveFileContext db)
        {
            List<ArchiveFile> files;
            
            // If the query parameter is the path to a text file that contains checksums.
            if (queryParameter.EndsWith(".txt"))
            {
                string[] checksums = getChecksums(queryParameter);

                files = db.Files.Where(f => checksums.Contains(f.Checksum)).ToList();
                Console.WriteLine(files.Count);
            }

            // If the query parameter is less than PUID_MAX_LENGTH chars, we have a puid.
            else if (queryParameter.Length < PUID_MAX_LENGTH)
            {

                files = db.Files.Where(f => f.Puid == queryParameter).ToList();
            }

            // Else, it must be a checksum.
            else
            {

                files = db.Files.Where(f => f.Checksum == queryParameter && db._ConvertedFiles.All(cf => cf.UUID != f.UUID)).ToList();
               
            }

            return files;
        }

    }
}
