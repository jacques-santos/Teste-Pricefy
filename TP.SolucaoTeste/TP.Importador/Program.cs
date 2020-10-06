using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using TP.Entity;
using System.Text;

namespace TP.Importador
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            bool terminado = false;
            while (!terminado)
            {
                Console.WriteLine("Selecione uma opção");
                Console.WriteLine("1- Importar arquivo");
                Console.WriteLine("2- Consultar arquivo por ID ou Nome");
                Console.WriteLine("3- Sair");
                string opcaoSelecionada = Console.ReadLine();

                switch (opcaoSelecionada)
                {
                    case "1":
                        ImportarArquivo();
                        break;
                    case "2":
                        ConsultarArquivo();
                        break;
                    case "3":
                        terminado = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida...");
                        break;
                }

                Console.WriteLine("\n\n");
            }
        }

        private async static void ImportarArquivo()
        {
            Console.WriteLine("Informe o caminho de um arquivo para importação:");
            string caminhoArquivoCSV = Console.ReadLine().Replace("\"", "");

            try
            {
                // Criação de StreamReader para importação via CSVHelper
                using (var fs = new StreamReader(caminhoArquivoCSV))
                {
                    List<SalesRecord> lines = new CsvReader(fs, System.Globalization.CultureInfo.CurrentCulture).GetRecords<LinhaCSV>().Select(l => new SalesRecord(l)).ToList();

                    Console.WriteLine("Informe um nome para identificação do arquivo:");
                    string NomeIdentificacao = Console.ReadLine();

                    // Preparação do objeto de arquivo com detalhes para envio
                    var csvFileDetails = new ArquivoCSV();
                    csvFileDetails.CaminhoInicial = caminhoArquivoCSV;
                    csvFileDetails.NomeIdentificacao = NomeIdentificacao;
                    csvFileDetails.TotalLinhas = lines.Count();
                    csvFileDetails.TotalLinhasComErro = lines.Count(l => l.FlagErro);
                    csvFileDetails.TotalLinhasImportadas = lines.Count(l => !l.FlagErro);
                    csvFileDetails.RegistrosParaImportacao = lines.Where(l => !l.FlagErro).ToList();

                    // Chamada à API de testes
                    client.BaseAddress = new Uri("http://localhost:57288/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Conversão para JSON do arquivo
                    string json = JsonSerializer.Serialize(csvFileDetails);

                    // Envio de dados e retorno esperado como JSON
                    HttpResponseMessage response = await client.PostAsync("api/principal/enviar", new StringContent(json, Encoding.UTF8, "application/json"));
                    var retorno = response.Content.ReadAsStringAsync().Result;                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async static void ConsultarArquivo()
        {
            Console.WriteLine("Informe o nome ou id do arquivo para consulta:");
            string dadoParaConsulta = Console.ReadLine();

            try
            {
                // Preparação do objeto de arquivo com detalhes para envio
                var csvFileDetails = new ArquivoCSV();
                
                var id = 0;
                int.TryParse(dadoParaConsulta, out id);

                csvFileDetails.IdCSVFile = id;
                csvFileDetails.NomeIdentificacao = dadoParaConsulta;
                
                // Chamada à API de testes
                client.BaseAddress = new Uri("http://localhost:57288/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Conversão para JSON do arquivo
                string json = JsonSerializer.Serialize(csvFileDetails);

                // Envio de dados e retorno esperado como JSON
                HttpResponseMessage response = await client.PostAsync("api/principal/consultar", new StringContent(json, Encoding.UTF8, "application/json"));
                var retorno = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
