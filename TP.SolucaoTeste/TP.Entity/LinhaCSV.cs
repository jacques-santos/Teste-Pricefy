using CsvHelper.Configuration.Attributes;
using System;

namespace TP.Entity
{
    public class LinhaCSV
    {
        [Name("Region")]
        public string Region { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Item Type")]
        public string Item_Type { get; set; }

        [Name("Sales Channel")]
        public string Sales_Channel { get; set; }

        [Name("Order Priority")]
        public string Order_Priority { get; set; }

        [Name("Order Date")]
        public string Order_Date { get; set; }

        [Name("Order ID")]
        public string Order_ID { get; set; }

        [Name("Ship Date")]
        public string Ship_Date { get; set; }

        [Name("Units Sold")]
        public string Units_Sold { get; set; }

        [Name("Unit Price")]
        public string Unit_Price { get; set; }

        [Name("Unit Cost")]
        public string Unit_Cost { get; set; }

        [Name("Total Revenue")]
        public string Total_Revenue { get; set; }

        [Name("Total Cost")]
        public string Total_Cost { get; set; }

        [Name("Total Profit")]
        public string Total_Profit { get; set; }
    }
}
