using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateHandler
{
    public class ConvertedFile
    {
        [Column("file_id")]
        public int ID { get; set; }

        [Column("uuid")]
        public string UUID { get; set; }

        public ConvertedFile(int iD, string uUID)
        {
            ID = iD;
            UUID = uUID;
        }
    }
}
