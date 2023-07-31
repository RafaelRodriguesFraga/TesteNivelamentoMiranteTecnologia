using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.ApiResult;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers.Commands
{
    public class MovimentoCommandHandler : IRequestHandler<MovimentoRequestCommand, ApiResult<MovimentoResponseCommand>>
    {
        private readonly IMovimentoWriteRepository _movimentoWriteRepository;
        private readonly IIdempotenciaWriteRepository _idempotenciaWriteRepository;
        private readonly IContaCorrenteReadRepository _contaCorrenteReadRepository;

        public MovimentoCommandHandler(IMovimentoWriteRepository movimentoWriteRepository, IIdempotenciaWriteRepository idempotenciaWriteRepository, IContaCorrenteReadRepository contaCorrenteReadRepository)
        {
            _movimentoWriteRepository = movimentoWriteRepository;
            _idempotenciaWriteRepository = idempotenciaWriteRepository;
            _contaCorrenteReadRepository = contaCorrenteReadRepository;
        }

        public async Task<ApiResult<MovimentoResponseCommand>> Handle(MovimentoRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.ValorMovimentado <= 0)
            {
                var resultado = ErroMovimentacao.INVALID_VALUE.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<MovimentoResponseCommand>(false, 400, dados: null, "Valor movimentado deve ser maior que zero");
            }

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            {
                var resultado = ErroMovimentacao.INVALID_TYPE.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<MovimentoResponseCommand>(false, 400, dados: null, "Tipo de movimento inválido. Escolha entre D para débito ou C para Crédito");
            }

            var contaCorrente = await _contaCorrenteReadRepository.ObterPorId(request.IdContaCorrente);
            if (contaCorrente == null)
            {
                var resultado = ErroMovimentacao.INVALID_ACCOUNT.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<MovimentoResponseCommand>(false, 400, dados: null, "Conta não existe");
            }

            var contaCorrenteAtiva = await _contaCorrenteReadRepository.ObterContaCorrenteAtiva(request.IdContaCorrente);
            if (contaCorrenteAtiva == null)
            {
                var resultado = ErroMovimentacao.INACTIVE_ACCOUNT.ToString();

                await _idempotenciaWriteRepository.InserirAsync(request, resultado);

                return new ApiResult<MovimentoResponseCommand>(false, 400, dados: null, "Conta inativa");
            }

            var movimentoId = await _movimentoWriteRepository.InserirMovimento(request.IdContaCorrente, request.TipoMovimento, request.ValorMovimentado);

            return new ApiResult<MovimentoResponseCommand>(true, 201, new MovimentoResponseCommand(movimentoId), null);
        }
    }
}
