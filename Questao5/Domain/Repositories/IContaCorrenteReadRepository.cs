using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories
{
    public interface IContaCorrenteReadRepository
    {
        Task<ContaCorrente> ObterPorId(string idContaCorrente);
        Task<ContaCorrente> ObterContaCorrenteAtiva(string idContaCorrente);
        Task<SaldoContaCorrente> ObterSaldoAtual(string idContaCorrente);
    }
}
