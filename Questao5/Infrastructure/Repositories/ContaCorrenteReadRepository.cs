using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteReadRepository : IContaCorrenteReadRepository
    {
        private readonly DatabaseConfig databaseConfig;
        SqliteConnection connection;

        public ContaCorrenteReadRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
            connection = new SqliteConnection(databaseConfig.Name);
            connection.Open();
        }

        public async Task<ContaCorrente> ObterContaCorrenteAtiva(string idContaCorrente)
        {
            try
            {
                string query = "SELECT * FROM contacorrente where IdContaCorrente = @IdContaCorrente and ativo = 1";

                var contaCorrenteAtiva = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { IdContaCorrente = idContaCorrente });

                return contaCorrenteAtiva;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<ContaCorrente> ObterPorId(string idContaCorrente)
        {
            try
            {
                string query = "SELECT * FROM contacorrente where IdContaCorrente = @IdContaCorrente";

                var contaCorrente = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { IdContaCorrente = idContaCorrente });
                
                return contaCorrente;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<SaldoContaCorrente> ObterSaldoAtual(string idContaCorrente)
        {
            try
            {
                string query = @"
                                SELECT
                                    cc.numero,
                                    cc.nome,
                                    (SUM(CASE WHEN m.tipomovimento = 'C' THEN m.valor ELSE 0 END)
	                                     - SUM(CASE WHEN m.tipomovimento = 'D' THEN m.valor ELSE 0 END)) AS saldoatual
                                    FROM contacorrente cc
                                    LEFT JOIN movimento m ON cc.idcontacorrente = m.idcontacorrente
                                    WHERE cc.idcontacorrente = @IdContaCorrente
                                    GROUP BY cc.numero, cc.nome";

                var contaCorrente = await connection.QueryFirstOrDefaultAsync<SaldoContaCorrente>(query, new { IdContaCorrente = idContaCorrente });

                return contaCorrente;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }            
        }
    }
}
