using Health_Med.Model;

namespace Health_Med.Repository.Interface
{
    public interface IConsultaRepository
    {
        Task<IEnumerable<Consulta>> ObterTodasAsync();
        Task<Consulta> ObterPorIdAsync(int id);
        Task<int> AgendarAsync(Consulta consulta);
        Task<bool> CancelarAsync(Consulta consulta);
        Task<bool> AceitarAsync(Consulta consulta);
    }
}
