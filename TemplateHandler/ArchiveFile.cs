using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateHandler
{
    /*
       The ArchiveFile class based on the schema for the Files table in the database.
        Each row in the Files table will be parsed as an ArchiveFile object with the Entity Framework.
     */
    public class ArchiveFile
    {

        [Column("id")]
        public int ID { get; set; }

        [Column("uuid")]
        public string UUID { get; set; }

        [Column("relative_path")]
        public string RelativePath { get; set; }

        [Column("checksum")]
        public string Checksum { get; set; }

        [Column("puid")]
        public string Puid  { get; set; }

        [Column("signature")]
        public string Signature { get; set;}

        [Column("is_binary")]
        public int IsBinary { get; set; }

        [Column("file_size_in_bytes")]
        public long Filesize;

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
    }
}
