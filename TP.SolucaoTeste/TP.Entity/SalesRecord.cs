using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TP.Entity
{
    public class SalesRecord
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string Item_Type { get; set; }
        public string Sales_Channel { get; set; }
        public string Order_Priority { get; set; }
        public DateTime Order_Date { get; set; }
        public int Order_ID { get; set; }
        public DateTime Ship_Date { get; set; }
        public int Units_Sold { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal Unit_Cost { get; set; }
        public decimal Total_Revenue { get; set; }
        public decimal Total_Cost { get; set; }
        public decimal Total_Profit { get; set; }

        public bool FlagErro { get; set; }
        public string ErroImportacao { get; set; }

        public SalesRecord()
        { }

        public SalesRecord(LinhaCSV oLinhaCSV)
        {
            ConverterDados(oLinhaCSV);
        }

        private void ConverterDados(LinhaCSV oLinhaCSV)
        {

            this.ErroImportacao = "";

            this.Region = oLinhaCSV.Region;
            this.Country = oLinhaCSV.Country;
            this.Item_Type = oLinhaCSV.Item_Type;
            this.Sales_Channel = oLinhaCSV.Sales_Channel;
            this.Order_Priority = oLinhaCSV.Order_Priority;

            DateTime Order_Date;
            if (DateTime.TryParse(oLinhaCSV.Order_Date, out Order_Date))
            { this.Order_Date = Order_Date; }
            else if (DateTime.TryParse(oLinhaCSV.Order_Date, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out Order_Date))
            { this.Order_Date = Order_Date; }
            else
            { this.ErroImportacao += "Valor de 'Order_Date' inválido;"; }

            int Order_ID;
            if (int.TryParse(oLinhaCSV.Order_ID, out Order_ID))
            { this.Order_ID = Order_ID; }
            else
            { this.ErroImportacao += "Valor de 'Order_ID' inválido;"; }

            DateTime Ship_Date;
            if (DateTime.TryParse(oLinhaCSV.Ship_Date, out Ship_Date))
            { this.Ship_Date = Ship_Date; }
            else if (DateTime.TryParse(oLinhaCSV.Ship_Date, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out Ship_Date))
            { this.Ship_Date = Ship_Date; }
            else
            { this.ErroImportacao += "Valor de 'Ship_Date' inválido;"; }

            int Units_Sold;
            if (int.TryParse(oLinhaCSV.Units_Sold, out Units_Sold))
            { this.Units_Sold = Units_Sold; }
            else
            { this.ErroImportacao += "Valor de 'Units_Sold' inválido;"; }

            decimal Unit_Price;
            if (decimal.TryParse(oLinhaCSV.Unit_Price, out Unit_Price))
            { this.Unit_Price = Unit_Price; }
            else
            { this.ErroImportacao += "Valor de 'Unit_Price' inválido;"; }

            decimal Unit_Cost;
            if (decimal.TryParse(oLinhaCSV.Unit_Cost, out Unit_Cost))
            { this.Unit_Cost = Unit_Cost; }
            else
            { this.ErroImportacao += "Valor de 'Unit_Cost' inválido;"; }

            decimal Total_Revenue;
            if (decimal.TryParse(oLinhaCSV.Total_Revenue, out Total_Revenue))
            { this.Total_Revenue = Total_Revenue; }
            else
            { this.ErroImportacao += "Valor de 'Total_Revenue' inválido;"; }

            decimal Total_Cost;
            if (decimal.TryParse(oLinhaCSV.Total_Cost, out Total_Cost))
            { this.Total_Cost = Total_Cost; }
            else
            { this.ErroImportacao += "Valor de 'Total_Cost' inválido;"; }

            decimal Total_Profit;
            if (decimal.TryParse(oLinhaCSV.Total_Profit, out Total_Profit))
            { this.Total_Profit = Total_Profit; }
            else
            { this.ErroImportacao += "Valor de 'Total_Profit' inválido;"; }

            this.FlagErro = this.ErroImportacao.Length > 0;
        }
    }
}
