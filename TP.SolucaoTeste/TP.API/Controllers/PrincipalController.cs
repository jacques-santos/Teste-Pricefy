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

        [HttpGet("consultar")]
        public List<ArquivoCSV> ConsultarArquivos([FromBody] ArquivoCSV arquivoCSV)
        {
            var listaArquivos = new List<ArquivoCSV>();

            try
            {   
                if(arquivoCSV.IdCSVFile > 0)
                { listaArquivos.Add(_dal.ObterArquivoCSVPorId(arquivoCSV.IdCSVFile)); }

                if (arquivoCSV.NomeIdentificacao.Length > 0)
                { listaArquivos.AddRange(_dal.ObterArquivoCSVPorNome(arquivoCSV.NomeIdentificacao));}

                return listaArquivos;
            }
            catch (Exception ex)
            {
                arquivoCSV.StatusProcessamento = -1;
                arquivoCSV.DescricaoProcessamento = ex.Message;
                return listaArquivos;
            }
        }
    }
}
