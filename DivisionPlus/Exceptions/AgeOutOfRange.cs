using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivisionPlus.Exceptions
{
    public class AgeOutOfRange : Exception
    {
        private int lowBount { get; set; }
        private int highBount { get; set; }
        public string StringBount { get; }
        public AgeOutOfRange() {
            StringBount = string.Empty;
        }

        public AgeOutOfRange(string message, int lowBount, int highBount) : base(message)
        {
            StringBount = "Від " + lowBount + " до " + highBount;
        }
    }
}
