using Health_Med.Model;

namespace Health_Med.Repository.Interface
{
    public interface IHorarioDisponivelRepository
    {
        Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId);
        Task<int> AdicionarAsync(HorarioDisponivel horario);
        Task<bool> RemoverAsync(int id);
        Task<HorarioDisponivel> ObterPorIdAsync(int id);
        Task<bool> AtualizarAsync(HorarioDisponivel horario);
    }
}
