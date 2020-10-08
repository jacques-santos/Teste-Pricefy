using System;
using System.Collections.Generic;
using System.Text;

namespace TP.Entity
{
    public class ConsultaArquivo
    {
        public int IdCSVFile { get; set; }        
        public string NomeIdentificacao { get; set; }
        public List<ArquivoCSV> ArquivosEncontrados { get; set; }
        public int StatusConsulta { get; set; }
        public string Descricao { get; set; }
    }
}
