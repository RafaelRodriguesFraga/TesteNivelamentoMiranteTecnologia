using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.ApiResult;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentoRequestCommand : IRequest<ApiResult<MovimentoResponseCommand>>
    {
        public string IdContaCorrente { get; set; }
        public decimal ValorMovimentado { get; set; }
        public string TipoMovimento { get; set; }
    }
}
