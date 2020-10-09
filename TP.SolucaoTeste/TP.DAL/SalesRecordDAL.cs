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
               new { idArquivoCSV });

                return arquivoCSV;
            }
        }

        public List<ArquivoCSV> ObterArquivoCSVPorNome(string nomeIdentificacao)
        {
            using (var connection = new SqlConnection(_conn))
            {
                var arquivoCSV = connection.Query<ArquivoCSV>(@"SELECT * FROM [dbo].[TbCSVFile] WHERE NomeIdentificacao like @nomeIdentificacao",
               new { nomeIdentificacao = $"%{nomeIdentificacao}%" }).ToList();

                return arquivoCSV;
            }
        }

        public ArquivoCSV SalvarRegistrosDoCSV(ArquivoCSV arquivoCSV)
        {
            try
            {

                using (var connection = new SqlConnection(_conn))
                {
                    arquivoCSV.IdCSVFile = connection.QuerySingle<int>(@"INSERT INTO [dbo].[TbCSVFile] (
                                                        [CaminhoInicial],
                                                        [NomeIdentificacao],      
                                                        [TotalLinhas],            
                                                        [TotalLinhasImportadas],  
                                                        [TotalLinhasComErro]
                                                        ) VALUES (
                                                        @CaminhoInicial,
                                                        @NomeIdentificacao,      
                                                        @TotalLinhas,            
                                                        @TotalLinhasImportadas,  
                                                        @TotalLinhasComErro
                                                        ); 
                                                       SELECT CAST(SCOPE_IDENTITY() as int)",
                    arquivoCSV);

                    arquivoCSV.RegistrosParaImportacao.ForEach(o => o.Fk_IdCSVFile = arquivoCSV.IdCSVFile);
                    DataTable dtRegistrosParaImportacao = CollectionHelper.ConvertTo<SalesRecord>(arquivoCSV.RegistrosParaImportacao.Where(l => !l.FlagErro).ToList());
                    DataTable dtRegistrosParaLog = CollectionHelper.ConvertTo<SalesRecord>(arquivoCSV.RegistrosParaImportacao.Where(l => l.FlagErro).ToList());

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

                    arquivoCSV.StatusProcessamento = 1;
                    arquivoCSV.DescricaoProcessamento = "Processo de importação executado com sucesso!";
                }

            }
            catch (Exception ex)
            {
                arquivoCSV.StatusProcessamento = -1;
                arquivoCSV.DescricaoProcessamento = $"Erro: {ex.Message}";
            }

            AtualizarStatusArquivoCSV(arquivoCSV);

            return arquivoCSV;

        }

        private void AtualizarStatusArquivoCSV(ArquivoCSV arquivoCSV)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Query(@"UPDATE [dbo].[TbCSVFile] SET StatusProcessamento = @StatusProcessamento,DescricaoProcessamento = @DescricaoProcessamento
                                                        WHERE IdCSVFile = @IdCSVFile; ",
                arquivoCSV);
            }
        }


    }
}
