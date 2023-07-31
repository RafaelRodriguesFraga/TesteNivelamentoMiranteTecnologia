using Questao5.Application.Commands.Requests;

namespace Questao5.Domain.Repositories
{
    public interface IIdempotenciaWriteRepository
    {
        Task InserirAsync(object request, string resultado);
    }
}
