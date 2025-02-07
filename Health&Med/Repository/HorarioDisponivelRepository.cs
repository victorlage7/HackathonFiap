using Dapper;
using System.Data;
using Health_Med.Repository.Interface;
using Health_Med.Model;

namespace Health_Med.Repository
{
    public class HorarioDisponivelRepository : IHorarioDisponivelRepository
    {
        private readonly IDbConnection _dbConnection;

        public HorarioDisponivelRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId)
        {
            var query = "SELECT * FROM HorariosDisponiveis WHERE MedicoId = @MedicoId";
            return await _dbConnection.QueryAsync<HorarioDisponivel>(query, new { MedicoId = medicoId });
        }

        public async Task<int> AdicionarAsync(HorarioDisponivel horario)
        {
            var query = @"INSERT INTO HorariosDisponiveis (MedicoId, DataHora, Disponivel) 
                          VALUES (@MedicoId, @DataHora, @Disponivel);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, horario);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var query = "DELETE FROM HorariosDisponiveis WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return result > 0;
        }
    }
}
