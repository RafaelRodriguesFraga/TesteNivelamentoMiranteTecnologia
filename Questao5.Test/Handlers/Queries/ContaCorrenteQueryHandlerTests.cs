using NSubstitute;
using Questao5.Application.Handlers.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Test.Handlers.Queries
{
    public class ContaCorrenteQueryHandlerTests
    {
        private readonly IContaCorrenteReadRepository _mockContaCorrenteReadRepository;
        private readonly IIdempotenciaWriteRepository _mockIdempotenciaWriteRepository;
        private readonly ContaCorrenteQueryHandler _contaCorrenteQueryHandler;

        public ContaCorrenteQueryHandlerTests()
        {
            _mockContaCorrenteReadRepository = Substitute.For<IContaCorrenteReadRepository>();
            _mockIdempotenciaWriteRepository = Substitute.For<IIdempotenciaWriteRepository>();
            _contaCorrenteQueryHandler = new ContaCorrenteQueryHandler(_mockContaCorrenteReadRepository, _mockIdempotenciaWriteRepository);
        }

        [Fact]
        public async Task QuandoContaCorrenteNaoExiste_DeveRetornarResultadoErro()
        {
            var request = new ContaCorrenteRequestQuery
            {
                IdContaCorrente = ""
            };

            _mockContaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns((ContaCorrente)null);

            var result = await _contaCorrenteQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal("Conta não existe", result.Erro);
            await _mockIdempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoContaCorrenteEstaInativa_DeveRetornarResultadoErro()
        {

            var request = new ContaCorrenteRequestQuery
            {
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };

            _mockContaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns(new ContaCorrente { IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca", Ativo = 0 });

            var result = await _contaCorrenteQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal(expected: "Conta inativa", result.Erro);
            await _mockIdempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoContaCorrenteExisteERetornaSaldo_DeveRetornarSaldoContaCorrente()
        {

            var request = new ContaCorrenteRequestQuery
            {
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };

            _mockContaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns(new ContaCorrente());
            _mockContaCorrenteReadRepository.ObterContaCorrenteAtiva(Arg.Any<string>()).Returns(new ContaCorrente { IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca", Ativo = 1 });
            _mockContaCorrenteReadRepository.ObterSaldoAtual(Arg.Any<string>()).Returns(new SaldoContaCorrente { SaldoAtual = 100 });

            var result = await _contaCorrenteQueryHandler.Handle(request, CancellationToken.None);

            Assert.True(result.Sucesso);
            Assert.Equal(200, result.Codigo);
            Assert.NotNull(result.Dados);
            Assert.Null(result.Erro);
            Assert.Equal(100, result.Dados.SaldoAtual);
            await _mockIdempotenciaWriteRepository.DidNotReceive().InserirAsync(Arg.Any<ContaCorrenteRequestQuery>(), Arg.Any<string>());
        }
    }
}
