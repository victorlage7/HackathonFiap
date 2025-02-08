using Dapper;
using Health_Med.Model;
using Health_Med.Repository.Interface;
using System.Data;

namespace Health_Med.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MedicoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Medico> ObterPorIdAsync(int id)
        {
            var query = "SELECT * FROM Medicos WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Medico>(query, new { Id = id });
        }

        public async Task<IEnumerable<Medico>> ObterTodosAsync()
        {
            var query = "SELECT * FROM Medicos";
            return await _dbConnection.QueryAsync<Medico>(query);
        }

        public async Task<int> AdicionarAsync(Medico medico)
        {
            var query = @"INSERT INTO Medicos (Nome, CRM, Email, Senha, Especilidade, ValorConsulta) 
                      VALUES (@Nome, @CRM, @Email, @Senha,@Especilidade, @ValorConsulta);
                      SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, medico);
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            var query = @"UPDATE Medicos SET 
                      Nome = @Nome, 
                      CRM = @CRM, 
                      Senha = @Senha,
                      Email = @Email,
                      Especilidade = @Especilidade,
                      ValorConsulta = @ValorConsulta
                      WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, medico);
            return result > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var query = "DELETE FROM Medicos WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Medico>> ObterPorEspecialdiadeAsync(int especialdiade)
        {
            var query = "SELECT * FROM Medicos where Especilidade = @Especilidade";
            return await _dbConnection.QueryAsync<Medico>(query, new { Especilidade = especialdiade });
        }
    }
}
