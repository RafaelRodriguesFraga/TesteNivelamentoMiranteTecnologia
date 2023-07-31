using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Responses
{
    public class MovimentoResponseCommand
    {
        public MovimentoResponseCommand(string idMovimento)
        {
            IdMovimento = idMovimento;
        }

        public string IdMovimento { get; set; }       
    }
}
