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
        static HttpClient client;

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
                        Functions.ImportarArquivo();
                        break;
                    case "2":
                        Functions.ConsultarArquivo();
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

       
    }
}
