using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories
{
    public interface IMovimentoWriteRepository
    {
        Task<string> InserirMovimento(string idContaCorrente, string tipoMovimento, decimal valor);
    }
}
