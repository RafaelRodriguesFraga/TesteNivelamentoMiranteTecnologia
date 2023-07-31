using Questao5.Domain.Entities;
using System.Globalization;

namespace Questao5.Application.Queries.Responses
{
    public class ContaCorrenteResponseQuery
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string DataHoraConsulta { get; set; }
        public decimal SaldoAtual { get; set; }

        public static explicit operator ContaCorrenteResponseQuery(SaldoContaCorrente contaCorrente)
        {            
            return new ContaCorrenteResponseQuery()
            {
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                DataHoraConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                SaldoAtual = contaCorrente.SaldoAtual
            };
        }
    }
}
