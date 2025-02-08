using Health_Med.Model;

namespace Health_Med.Repository.Interface
{
    public interface IMedicoRepository
    {
        Task<Medico> ObterPorIdAsync(int id);
        Task<IEnumerable<Medico>> ObterTodosAsync();
        Task<int> AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> RemoverAsync(int id);
        Task<IEnumerable<Medico>> ObterPorEspecialdiadeAsync(int especialdiade);
    }
}
