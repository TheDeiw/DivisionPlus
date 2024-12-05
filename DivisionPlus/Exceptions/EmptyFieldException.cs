using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivisionPlus.Exceptions
{
    public class EmptyFieldException : Exception
    {
        public string FieldName { get; set; }

        public EmptyFieldException()
        {
            FieldName = string.Empty;
        }

        public EmptyFieldException(string? Message, string FieldName) : base(Message)
        {
            this.FieldName = FieldName;
        }
    }
}
