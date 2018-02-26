using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkaOreo
{
    class CompanyItemData
    {
        public string CompanyPrefix { get; }
        public string CompanyName { get; }
        public string ItemReference { get; }
        public string ItemName { get; }

        public CompanyItemData(string companyPrefix, string companyName, string itemReference, string itemName)
        {
            this.CompanyPrefix = companyPrefix;
            this.CompanyName = companyName;
            this.ItemReference = itemReference;
            this.ItemName = itemName;
        }

        public override string ToString()
        {
            return CompanyPrefix + ";" + CompanyName + ";" + ItemReference + ";" + ItemName;
        }
    }
}
