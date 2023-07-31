using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoWriteRepository : IMovimentoWriteRepository
    {
        private readonly DatabaseConfig databaseConfig;
        SqliteConnection connection;

        public MovimentoWriteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
            connection = new SqliteConnection(databaseConfig.Name);
            connection.Open();
        }

        public async Task<string> InserirMovimento(string idContaCorrente, string tipoMovimento, decimal valor)
        {
            try
            {
                var insertSql = @"INSERT INTO movimento
                                (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                             VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";

                Movimento movimento = new Movimento(idContaCorrente, tipoMovimento, valor);              

                await connection.ExecuteAsync( insertSql, movimento);          

                return movimento.IdMovimento;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);

            }finally
            {
                connection.Close();
            }
         
        }
    }
}
