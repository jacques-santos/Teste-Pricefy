using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TP.Entity;

namespace TP.Importador
{
    public class Functions
    {
        public static ArquivoCSV ImportarArquivo(string caminhoArquivoCSV = null, string nomeIdentificacao = null)
        {
            Console.WriteLine("Informe o caminho de um arquivo para importação:");
            if (string.IsNullOrEmpty(caminhoArquivoCSV))
            { caminhoArquivoCSV = Console.ReadLine().Replace("\"", ""); }

            try
            {
                // Criação de StreamReader para importação via CSVHelper
                using (var fs = new StreamReader(caminhoArquivoCSV))
                {
                    List<SalesRecord> lines = new CsvReader(fs, System.Globalization.CultureInfo.CurrentCulture).GetRecords<LinhaCSV>().Select((l, index) => new SalesRecord(l, index)).ToList();

                    Console.WriteLine("Informe um nome para identificação do arquivo:");
                    if (string.IsNullOrEmpty(nomeIdentificacao))
                    { nomeIdentificacao = Console.ReadLine(); }

                    // Preparação do objeto de arquivo com detalhes para envio
                    var csvFileDetails = new ArquivoCSV();
                    csvFileDetails.CaminhoInicial = caminhoArquivoCSV;
                    csvFileDetails.NomeIdentificacao = nomeIdentificacao;
                    csvFileDetails.TotalLinhas = lines.Count();
                    csvFileDetails.TotalLinhasComErro = lines.Count(l => l.FlagErro);
                    csvFileDetails.TotalLinhasImportadas = lines.Count(l => !l.FlagErro);
                    csvFileDetails.RegistrosParaImportacao = lines.ToList();

                    // Chamada à API de testes
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:5000/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Conversão para JSON do arquivo
                    string json = JsonSerializer.Serialize(csvFileDetails);

                    // Envio de dados e retorno esperado como JSON
                    HttpResponseMessage retorno = client.PostAsync("api/principal/enviar", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    var arquivoRetorno = JsonSerializer.Deserialize<ArquivoCSV>(retorno.Content.ReadAsStringAsync().Result);

                    if (arquivoRetorno.StatusProcessamento == -1)
                    { Console.WriteLine("Erro na Importação pela API: " + arquivoRetorno.DescricaoProcessamento); }
                    else
                    {
                        Console.WriteLine("Status de Importação:");
                        Console.WriteLine($"---Total de Linhas:{arquivoRetorno.TotalLinhas}");
                        Console.WriteLine($"---Total de Linhas Importadas:{arquivoRetorno.TotalLinhasImportadas}");
                        Console.WriteLine($"---Total de Linhas Com Erro:{arquivoRetorno.TotalLinhasComErro}");
                        Console.WriteLine($"---ID do Arquivo:{arquivoRetorno.IdCSVFile}");
                        Console.WriteLine($"---Nome de Identificação:{arquivoRetorno.NomeIdentificacao}");
                    }

                    return arquivoRetorno;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro geral: " + e);
                return null;
            }
        }
      
        public static  ConsultaArquivo ConsultarArquivo(string dadoParaConsulta = null)
        {
            Console.WriteLine("Informe o nome ou id do arquivo para consulta:");

            if (string.IsNullOrEmpty(dadoParaConsulta))
            { dadoParaConsulta = Console.ReadLine(); }

            try
            {
                // Preparação do objeto de arquivo com detalhes para envio
                var consulta = new ConsultaArquivo();

                var id = 0;
                int.TryParse(dadoParaConsulta, out id);

                consulta.IdCSVFile = id;
                consulta.NomeIdentificacao = dadoParaConsulta;

                // Chamada à API de testes
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Conversão para JSON do arquivo
                string json = JsonSerializer.Serialize(consulta);

                // Envio de dados e retorno esperado como JSON
                HttpResponseMessage retorno = client.PostAsync("api/principal/consultar", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                var retornoConsulta = JsonSerializer.Deserialize<ConsultaArquivo>(retorno.Content.ReadAsStringAsync().Result);

                Console.WriteLine($"\n\n{retornoConsulta.Descricao};");

                foreach (var arq in retornoConsulta.ArquivosEncontrados.Select((value, i) => new { i, value }))
                {
                    Console.WriteLine($"\nDados do arquivo ({arq.i}):");
                    Console.WriteLine($"---Total de Linhas:{arq.value.TotalLinhas}");
                    Console.WriteLine($"---Total de Linhas Importadas:{arq.value.TotalLinhasImportadas}");
                    Console.WriteLine($"---Total de Linhas Com Erro:{arq.value.TotalLinhasComErro}");
                    Console.WriteLine($"---ID do Arquivo:{arq.value.IdCSVFile}");
                    Console.WriteLine($"---Nome de Identificação:{arq.value.NomeIdentificacao}");
                    Console.WriteLine($"---Data Processamento:{arq.value.DataInsercao.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"---Descricao Processamento:{arq.value.DescricaoProcessamento}");
                }

                return retornoConsulta;
            }
            catch (Exception e)
            {                
                Console.WriteLine(e);
                return null;
            }
        }

       
    }
}
