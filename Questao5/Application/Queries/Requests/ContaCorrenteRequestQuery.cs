using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Application.ApiResult;

namespace Questao5.Application.Queries.Requests
{
    public class ContaCorrenteRequestQuery : IRequest<ApiResult<ContaCorrenteResponseQuery>>
    {
        public ContaCorrenteRequestQuery()
        {
                
        }
        public ContaCorrenteRequestQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }

        public string IdContaCorrente { get; set; }
    }
}
