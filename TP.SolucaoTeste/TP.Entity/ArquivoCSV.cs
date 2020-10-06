using System;
using System.Collections.Generic;
using System.Text;

namespace TP.Entity
{
    public class ArquivoCSV
    {
        public int IdCSVFile { get; set; }
        public string CaminhoInicial { get; set; }
        public string NomeIdentificacao { get; set; }
        public int TotalLinhas { get; set; }
        public int TotalLinhasImportadas { get; set; }
        public int TotalLinhasComErro { get; set; }
        public int StatusProcessamento { get; set; }
        public string DescricaoProcessamento { get; set; }
        public List<SalesRecord> RegistrosParaImportacao { get; set; }
    }
}
