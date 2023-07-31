using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers.Commands;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Test.Handlers.Commands
{
    public class MovimentoCommandHandlerTests
    {
        private readonly IMovimentoWriteRepository _movimentoWriteRepository;
        private readonly IIdempotenciaWriteRepository _idempotenciaWriteRepository;
        private readonly IContaCorrenteReadRepository _contaCorrenteReadRepository;
        private readonly MovimentoCommandHandler _movimentoCommandHandler;

        public MovimentoCommandHandlerTests()
        {
            _movimentoWriteRepository = Substitute.For<IMovimentoWriteRepository>();
            _idempotenciaWriteRepository = Substitute.For<IIdempotenciaWriteRepository>();
            _contaCorrenteReadRepository = Substitute.For<IContaCorrenteReadRepository>();
            _movimentoCommandHandler = new MovimentoCommandHandler(_movimentoWriteRepository, _idempotenciaWriteRepository, _contaCorrenteReadRepository);
        }

        [Fact]
        public async Task QuandoValorMovimentadoForInvalido_DeveRetornarResultadoErro()
        {
            var request = new MovimentoRequestCommand
            {
                ValorMovimentado = -100,
                TipoMovimento = "C",
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };

            var result = await _movimentoCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal("Valor movimentado deve ser maior que zero", result.Erro);

            await _idempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoTipoMovimentoForInvalido_DeveRetornarResultadoErro()
        {

            var request = new MovimentoRequestCommand
            {
                ValorMovimentado = 100,
                TipoMovimento = "E",
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };

            var result = await _movimentoCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal("Tipo de movimento inválido. Escolha entre D para débito ou C para Crédito", result.Erro);

            await _idempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoContaCorrenteForInvalida_DeveRetornarResultadoErro()
        {
            var request = new MovimentoRequestCommand
            {
                ValorMovimentado = 100,
                TipoMovimento = "C",
                IdContaCorrente = ""
            };

            _contaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns((ContaCorrente)null);

            var result = await _movimentoCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal("Conta não existe", result.Erro);

            await _idempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoContaCorrenteForInativa_DeveRetornarResultadoErro()
        {
            var request = new MovimentoRequestCommand
            {
                ValorMovimentado = 100,
                TipoMovimento = "C",
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };
            _contaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns(new ContaCorrente { Ativo = 0 });

            var result = await _movimentoCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(result.Sucesso);
            Assert.Equal(400, result.Codigo);
            Assert.Null(result.Dados);
            Assert.Equal("Conta inativa", result.Erro);

            await _idempotenciaWriteRepository.Received(1).InserirAsync(Arg.Is(request), Arg.Any<string>());
        }

        [Fact]
        public async Task QuandoRequisicaoForValida_DeveRetornarResultadoSucesso()
        {

            var request = new MovimentoRequestCommand
            {
                ValorMovimentado = 100,
                TipoMovimento = "C",
                IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca"
            };

            _contaCorrenteReadRepository.ObterPorId(Arg.Any<string>()).Returns(returnThis: new ContaCorrente { IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca", Ativo = 1 });
            _contaCorrenteReadRepository.ObterContaCorrenteAtiva(Arg.Any<string>()).Returns(new ContaCorrente { IdContaCorrente = "5dd72bb6-ec17-45d9-836f-2cecd4ff9dca", Ativo = 1 });
            _movimentoWriteRepository.InserirMovimento(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>()).Returns("058189c2-f7e9-4c25-8f4a-b58b2fdd2710");

            var result = await _movimentoCommandHandler.Handle(request, CancellationToken.None);

            Assert.True(result.Sucesso);
            Assert.Equal(201, result.Codigo);
            Assert.NotNull(result.Dados);
            Assert.Null(result.Erro);
            Assert.Equal("058189c2-f7e9-4c25-8f4a-b58b2fdd2710", result.Dados.IdMovimento);

            await _idempotenciaWriteRepository.DidNotReceive().InserirAsync(Arg.Any<MovimentoRequestCommand>(), Arg.Any<string>());
        }
    }
}