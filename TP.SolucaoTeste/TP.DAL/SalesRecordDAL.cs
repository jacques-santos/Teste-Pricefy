using System;
using System.Collections.Generic;
using System.Text;
using TP.Entity;

namespace TP.DAL
{
    public class SalesRecordDAL
    {
        public ArquivoCSV ObterArquivoCSVPorId(int idArquivoCSV)
        { return new ArquivoCSV(); }

        public List<ArquivoCSV> ObterArquivoCSVPorNome(string nomeIdentificacao)
        { return new List<ArquivoCSV>(); }

        public ArquivoCSV SalvarRegistrosDoCSV(ArquivoCSV arquivoCSV)
        { return new ArquivoCSV(); }

    }
}
