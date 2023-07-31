using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Questao5.Application.ApiResult;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Application.Handlers.Queries
{
    public class ContaCorrenteQueryHandler : IRequestHandler<ContaCorrenteRequestQuery, ApiResult<ContaCorrenteResponseQuery>>
    {
        private readonly IContaCorrenteReadRepository _contaCorrenteReadRepository;
        private readonly IIdempotenciaWriteRepository _idempotenciaWriteRepository;

        public ContaCorrenteQueryHandler(IContaCorrenteReadRepository contaCorrenteReadRepository, IIdempotenciaWriteRepository idempotenciaWriteRepository)
        {
            _contaCorrenteReadRepository = contaCorrenteReadRepository;
            _idempotenciaWriteRepository = idempotenciaWriteRepository;
        }

        public async Task<ApiResult<ContaCorrenteResponseQuery>> Handle(ContaCorrenteRequestQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteReadRepository.ObterPorId(request.IdContaCorrente);
            if (contaCorrente == null)
            {
                var resultado = ErroMovimentacao.INVALID_ACCOUNT.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<ContaCorrenteResponseQuery>(false, 400, dados: null, "Conta não existe");
            }

            var contaCorrenteAtiva = await _contaCorrenteReadRepository.ObterContaCorrenteAtiva(request.IdContaCorrente);
            if (contaCorrenteAtiva == null)
            {
                var resultado = ErroMovimentacao.INACTIVE_ACCOUNT.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<ContaCorrenteResponseQuery>(false, 400, dados: null, "Conta inativa");
            }

            var saldoContaCorrente = await _contaCorrenteReadRepository.ObterSaldoAtual(request.IdContaCorrente);

            return new ApiResult<ContaCorrenteResponseQuery>(true, 200, (ContaCorrenteResponseQuery)saldoContaCorrente, null);
        }
    }
}
