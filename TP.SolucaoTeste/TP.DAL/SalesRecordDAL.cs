using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TP.DAL.Helpers;
using TP.Entity;
using Dapper;
using System.Linq;

namespace TP.DAL
{
    public class SalesRecordDAL
    {
        private string _conn;
        public SalesRecordDAL(string conn)
        {
            _conn = conn;
        }

        public ArquivoCSV ObterArquivoCSVPorId(int idArquivoCSV)
        {
            using (var connection = new SqlConnection(_conn))
            {
                var arquivoCSV = connection.QuerySingle<ArquivoCSV>(@"SELECT * FROM [dbo].[TbCSVFile] WHERE IdCSVFile = @idArquivoCSV",
               idArquivoCSV);

                return arquivoCSV;
            }
        }

        public List<ArquivoCSV> ObterArquivoCSVPorNome(string nomeIdentificacao)
        {
            using (var connection = new SqlConnection(_conn))
            {
                var arquivoCSV = connection.Query<ArquivoCSV>(@"SELECT * FROM [dbo].[TbCSVFile] WHERE NomeIdentificacao = @nomeIdentificacao",
               nomeIdentificacao).ToList();

                return arquivoCSV;
            }
        }

        public ArquivoCSV SalvarRegistrosDoCSV(ArquivoCSV arquivoCSV)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();

                arquivoCSV.IdCSVFile = connection.QuerySingle<int>(@"INSERT INTO [dbo].[TbCSVFile] (
                                                        [CaminhoInicial],
                                                        [NomeIdentificacao],      
                                                        [TotalLinhas],            
                                                        [TotalLinhasImportadas],  
                                                        [TotalLinhasComErro],     
                                                        [StatusProcessamento],    
                                                        [DescricaoProcessamento]
                                                        ) VALUES (
                                                        @CaminhoInicial,
                                                        @NomeIdentificacao,      
                                                        @TotalLinhas,            
                                                        @TotalLinhasImportadas,  
                                                        @TotalLinhasComErro,     
                                                        @StatusProcessamento,    
                                                        @DescricaoProcessamento
                                                        ); 
                                                       SELECT CAST(SCOPE_IDENTITY() as int)",
                arquivoCSV);

                arquivoCSV.RegistrosParaImportacao.ForEach(o => o.Fk_IdCSVFile = arquivoCSV.IdCSVFile);
                DataTable dtRegistrosParaImportacao = CollectionHelper.ConvertTo(arquivoCSV.RegistrosParaImportacao.Where(l => !l.FlagErro));
                DataTable dtRegistrosParaLog = CollectionHelper.ConvertTo(arquivoCSV.RegistrosParaImportacao.Where(l => l.FlagErro));

                // Bulk Copy para Importacao
                SqlBulkCopy objBulkImportacao = new SqlBulkCopy(_conn);
                objBulkImportacao.DestinationTableName = "TbSalesRecord";
                string[] colunasImportacao = {"Fk_IdCSVFile",
                                    "Region",
                                    "Country",
                                    "Item_Type",
                                    "Sales_Channel",
                                    "Order_Priority",
                                    "Order_Date",
                                    "Order_ID",
                                    "Ship_Date",
                                    "Units_Sold",
                                    "Unit_Price",
                                    "Unit_Cost",
                                    "Total_Revenue",
                                    "Total_Cost",
                                    "Total_Profit",
                                    "NumLinha" };
                foreach (var column in colunasImportacao)
                { objBulkImportacao.ColumnMappings.Add(column, column); }
                objBulkImportacao.WriteToServer(dtRegistrosParaImportacao);

                // Bulk Copy para Log de Erros
                SqlBulkCopy objBulkLog = new SqlBulkCopy(_conn);
                objBulkLog.DestinationTableName = "TbLogImportacao";
                string[] colunasLog = {"Fk_IdCSVFile",
                                    "NumLinha",
                                    "ErroImportacao"};

                foreach (var col in colunasLog)
                { objBulkLog.ColumnMappings.Add(col, col); }
                objBulkLog.WriteToServer(dtRegistrosParaLog);
            }

            return new ArquivoCSV();
        }

    }
}
