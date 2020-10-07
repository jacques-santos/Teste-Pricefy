using System;
using System.Collections.Generic;
using System.Linq;
using TP.Entity;
using TP.Importador;
using Xunit;

namespace TP.XUnitTest
{
    public class UnitTest
    {

        [Fact]
        public void TesteDeImportacaoDeArquivoLocal()
        {
            var retornoDoTeste = Functions.ImportarArquivo("\"C:\\Users\\Vera\\Desktop\\10000 Sales Records.csv\"", $"Teste_{DateTime.Now.ToString("yyMMddHHmmss")}");

            Assert.Equal(expected: 1, actual: retornoDoTeste.StatusProcessamento);
        }

        [Fact]
        public void TesteDeConsultaSemRetorno()
        {
            var retornoDoTeste = Functions.ConsultarArquivo("584545454545152188");
            Assert.Equal(expected: new List<ArquivoCSV>(), actual: retornoDoTeste);
        }

        [Fact]
        public void TesteDeConsultaComRetorno()
        {
            var retornoDoTeste = Functions.ConsultarArquivo("Teste");
            Assert.True(retornoDoTeste.Any());
        }
    }
}
