using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkaOreo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string[] dataLines = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\data.csv");
            string lineMilka = dataLines.Where(l => l.Contains("Milka Oreo")).FirstOrDefault();
            if (lineMilka == null)
            {
                Console.WriteLine("There is no data about Milka Oreo.");
                return;
            }

            string[] lineSplitted = lineMilka.Split(';');
            if (lineSplitted.Length != 4)
            {
                Console.WriteLine("The data about Milka Oreo were not correctly formatted.");
                return;
            }

            CompanyItemData milkaData = new CompanyItemData(lineSplitted[0], lineSplitted[1], lineSplitted[2], lineSplitted[3]);
            Console.WriteLine(milkaData);

            string[] sgtin64Epcs = File.ReadAllLines(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\tags.txt");
            int milkaOreoCounter = 0;
            int invalidSgtinCounter = 0;

            foreach (string line in sgtin64Epcs)
            {
                if (!string.Empty.Equals(line.Trim()))
                {
                    Sgtin sgtin = new Sgtin96(line);
                    if (sgtin.Validate() && sgtin.Parse())
                    {
                        if (sgtin.GS1CompanyPrefix.Equals(milkaData.CompanyPrefix) && sgtin.ItemReference.Equals(milkaData.ItemReference))
                        {
                            milkaOreoCounter++;
                            Console.WriteLine("{0}) Milka Oreo found! Serial number: {1}", milkaOreoCounter, sgtin.SerialReference);
                        }
                    }
                    else
                    {
                        invalidSgtinCounter++;
                        Console.WriteLine("Invalid SGTIN-96: " + line);
                    }
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("There are {0} Milka Oreo chocolates!", milkaOreoCounter);
            Console.WriteLine("There are {0} invalid SGTIN-96 EPCs.", invalidSgtinCounter);
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
