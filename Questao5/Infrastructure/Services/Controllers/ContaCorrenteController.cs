using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.ApiResult;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém o saldo de uma conta corrente
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     GET /ContaCorrente/2029221a-04e6-46d0-8dd7-84489c680d79
        /// 
        /// Exemplo de resposta com sucesso:
        /// 
        ///     {
        ///        "sucesso": true,
        ///        "codigo": 200,
        ///        "dados": {
        ///           "numero": 123,
        ///           "nome": "Rafael Fraga",
        ///           "dataHoraConsulta": "30/07/2023 10:37",
        ///           "saldoAtual": 340
        ///        },
        ///        "erro": null
        ///     }
        ///     
        /// Exemplo de resposta com erro: 
        /// 
        ///     {
        ///        "sucesso": false,
        ///        "codigo": 400,
        ///        "dados": null,
        ///        "erro": "Conta inativa"
        ///     }
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Saldo da Conta Corrente</returns>
        /// <response code="200">Retorna o saldo da conta corrente criada</response>
        /// <response code="400">Retorna erro caso aconteça algum problema como Conta Inativa ou Inválida</response>  
        [HttpGet("{idContaCorrente}")]

        public async Task<IActionResult> ObterSaldoAsync([FromRoute] string idContaCorrente)
        {
            var resultado = await _mediator.Send(new ContaCorrenteRequestQuery(idContaCorrente));
            if (!resultado.Sucesso)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}