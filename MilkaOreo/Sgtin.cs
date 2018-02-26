using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkaOreo
{
    public abstract class Sgtin
    {
        public Sgtin(string sgtinHex, int sgtinLength, string header)
        {
            this.SgtinHex = sgtinHex;
            this.SgtinLength = sgtinLength;
            this.Header = header;
        }

        public string SgtinHex { get; protected set; }

        public string SgtinBin { get; protected set; }

        public string Header { get; protected set; }

        public int Filter { get; protected set; }

        // long value formatted to contain leading zeroes if necessary
        public string GS1CompanyPrefix { get; protected set; }

        // long value formatted to contain leading zeroes if necessary
        public string ItemReference { get; protected set; }

        public string SerialReference { get; protected set; }

        protected int SgtinLength { get; set; }

        /// <summary>
        /// Indicates whether <paramref name="SgtinHex"/> is a valid hexadecimal
        /// number with the required length and whether it has the required header
        /// </summary>
        /// <returns>true if all the conditions are satisfied; otherwise, false.</returns>
        public bool Validate()
        {
            if (!(this.SgtinHex.Length == this.SgtinLength / 4))
            {
                return false;
            }

            if (!this.SgtinHex.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
            {
                return false;
            }

            if (!this.SgtinHex.StartsWith(this.Header))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts <paramref name="SgtinHex"/> to its binary representation <paramref name="SgtinBin"/>.
        /// Parses the binary representation to get
        /// <paramref name="Filter"/>,
        /// <paramref name="GS1CompanyPrefix"/>,
        /// <paramref name="ItemReference"/>,
        /// <paramref name="SerialReference"/>.
        /// </summary>
        /// <returns>true if the specified sgtin was successfully parsed; otherwise, false.</returns>
        public abstract bool Parse();
    }
}
