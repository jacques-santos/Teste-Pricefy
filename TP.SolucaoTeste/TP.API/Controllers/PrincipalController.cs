using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TP.DAL;
using TP.Entity;

namespace TP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly ILogger<PrincipalController> _logger;
        private readonly IConfiguration _configuration;
        private SalesRecordDAL _dal;

        public PrincipalController(ILogger<PrincipalController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _dal = new SalesRecordDAL(_configuration.GetConnectionString("TestePricefyDatabase"));
        }

        [HttpPost("enviar")]
        public ActionResult<ArquivoCSV> EnviarCSVSalesRecord([FromBody] ArquivoCSV arquivoCSV)
        {
            try
            {
                _dal.SalvarRegistrosDoCSV(arquivoCSV);
                return arquivoCSV;
            }
            catch (Exception ex)
            {
                arquivoCSV.StatusProcessamento = -1;
                arquivoCSV.DescricaoProcessamento = ex.Message;
                return arquivoCSV;
            }
        }

        [HttpPost("consultar")]
        public ConsultaArquivo ConsultarArquivos([FromBody] ConsultaArquivo consulta)
        {
            try
            {
                consulta.ArquivosEncontrados = new List<ArquivoCSV>();

                if (consulta.IdCSVFile > 0)
                { consulta.ArquivosEncontrados.Add(_dal.ObterArquivoCSVPorId(consulta.IdCSVFile)); }

                if (consulta.NomeIdentificacao.Length > 0)
                { consulta.ArquivosEncontrados.AddRange(_dal.ObterArquivoCSVPorNome(consulta.NomeIdentificacao));}

                consulta.StatusConsulta = 1;
                consulta.Descricao = $"({consulta.ArquivosEncontrados.Count()}) Arquivos Encontrados";

                return consulta;
            }
            catch (Exception ex)
            {
                consulta.StatusConsulta = -1;
                consulta.Descricao = $"Erro: {ex.Message}";
                return consulta;
            }
        }
    }
}
