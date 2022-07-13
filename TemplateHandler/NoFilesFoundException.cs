using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateHandler
{
    /*
      Custom exception to throw when no files where found in the 
      database based on the specified query parameter.
     */
    internal class NoFilesFoundException : Exception
    {
        public NoFilesFoundException()
        {
        }

        public NoFilesFoundException(string message)
        : base(message)
        {
        }

        public NoFilesFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
