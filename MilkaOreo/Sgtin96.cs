using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkaOreo
{
    public class Sgtin96 : Sgtin
    {
        private static readonly Dictionary<int, PartitionValueData> PartitionDefinition = new Dictionary<int, PartitionValueData> {
            { 0, new PartitionValueData(40, 12, 4, 1) },
            { 1, new PartitionValueData(37, 11, 7, 2) },
            { 2, new PartitionValueData(34, 10, 10, 3) },
            { 3, new PartitionValueData(30, 9, 14, 4) },
            { 4, new PartitionValueData(27, 8, 17, 5) },
            { 5, new PartitionValueData(24, 7, 20, 6) },
            { 6, new PartitionValueData(20, 6, 24, 7) }
        };

        private int partition;

        public Sgtin96(string sgtinHex)
            : base(sgtinHex, 96, "30")
        {
        }

        public override bool Parse()
        {
            this.SgtinBin = string.Join(string.Empty, this.SgtinHex.Select(
              c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
              ));

            // first 8 bits are reserved for the header
            // next 3 bits represent the filter
            this.Filter = Convert.ToInt16(this.SgtinBin.Substring(8, 3));

            this.partition = Convert.ToInt16(this.SgtinBin.Substring(11, 3), 2);
            if (this.partition > 6)
            {
                return false;
            }

            PartitionValueData partitionValueData = PartitionDefinition[this.partition];

            int gs1CompanyPrefixBits = partitionValueData.Gs1CompanyPrefixBits;
            int gs1CompanyPrefixDigits = partitionValueData.Gs1CompanyPrefixDigits;
            string gs1CompanyPrefixBin = this.SgtinBin.Substring(14, gs1CompanyPrefixBits);
            this.GS1CompanyPrefix = Convert.ToInt64(gs1CompanyPrefixBin, 2).ToString().PadLeft(gs1CompanyPrefixDigits, '0');
            if (this.GS1CompanyPrefix.Length > gs1CompanyPrefixDigits)
            {
                return false;
            }

            int itemReferenceBits = partitionValueData.ItemReferenceBits;
            int itemReferenceDigits = partitionValueData.ItemReferenceDigits;
            string itemReferenceBin = this.SgtinBin.Substring(14 + gs1CompanyPrefixBits, itemReferenceBits);
            this.ItemReference = Convert.ToInt64(itemReferenceBin, 2).ToString().PadLeft(itemReferenceDigits, '0');
            if (this.ItemReference.Length > itemReferenceDigits)
            {
                return false;
            }

            int serialStart = 14 + gs1CompanyPrefixBits + itemReferenceBits;
            string serialReferenceBin = this.SgtinBin.Substring(serialStart, this.SgtinBin.Length - serialStart);
            this.SerialReference = Convert.ToInt64(serialReferenceBin, 2).ToString();

            return true;
        }

        private struct PartitionValueData
        {
            public short Gs1CompanyPrefixBits;
            public short Gs1CompanyPrefixDigits;
            public short ItemReferenceBits;
            public short ItemReferenceDigits;

            public PartitionValueData(short gs1CompanyPrefixBits, short gs1CompanyPrefixDigits, short itemReferenceBits, short itemReferenceDigits)
                : this()
            {
                this.Gs1CompanyPrefixBits = gs1CompanyPrefixBits;
                this.Gs1CompanyPrefixDigits = gs1CompanyPrefixDigits;
                this.ItemReferenceBits = itemReferenceBits;
                this.ItemReferenceDigits = itemReferenceDigits;
            }
        }
    }
}
