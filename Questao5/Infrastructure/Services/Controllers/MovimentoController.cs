using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentoController : ControllerBase
    {
        private IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Obtém o id do movimento da conta corrente
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     {
        ///        "idContaCorrente": "6b0094f9-05e4-4f08-85cd-08bb58329e13",
        ///        "valorMovimentado": 50,
        ///        "tipoMovimento": "D"
        ///     }
        /// 
        /// Exemplo de resposta com sucesso:
        /// 
        ///     {
        ///        "sucesso": true,
        ///        "codigo": 201,
        ///        "dados": {
        ///           "idMovimento": "9e0430e9-a26c-4af8-94e8-d810ef048e3f"
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
        /// <param name="value">command</param>
        /// <returns>Saldo da Conta Corrente</returns>
        /// <response code="201">Retorna o id do movimento criado</response>
        /// <response code="400">Retorna erro caso aconteça algum problema como Conta Inativa ou Inválida</response>  
        [HttpPost]
        public async Task<IActionResult> InserirMovimentacaoAsync(MovimentoRequestCommand command)
        {
            var resultado = await _mediator.Send(command);
            if (!resultado.Sucesso)
            {
                return BadRequest(resultado);
            }

            return Created("", resultado);
        }
    }
}