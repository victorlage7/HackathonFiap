using Microsoft.AspNetCore.Mvc;
using Health_Med.Repository.Interface;
using Health_Med.Model;
using Microsoft.AspNetCore.Authorization;
using Health_Med.VOs;

[ApiController]
[Route("api/[controller]")]
public class HorarioDisponivelController : ControllerBase
{
    private readonly IHorarioDisponivelRepository _horarioRepository;

    private readonly IMedicoRepository _medicoRepository;

    public HorarioDisponivelController(IHorarioDisponivelRepository horarioRepository,IMedicoRepository medicoRepository)
    {
        _horarioRepository = horarioRepository;
        _medicoRepository = medicoRepository;
    }

    [Authorize(Roles = "Medico,Paciente")]
    [HttpGet("medico/{medicoId}")]
    public async Task<IActionResult> ObterPorMedico(int medicoId)
    {
        try
        {
            Medico medico = await _medicoRepository.ObterPorIdAsync(medicoId);
            var horarios = await _horarioRepository.ObterPorMedicoAsync(medicoId);
            
            List<HorarioDisponivelVO> listHorarioDisponivelVO = new List<HorarioDisponivelVO>();

            foreach (var item in horarios)
            {
                HorarioDisponivel horarioDisponivel = new HorarioDisponivel()
                {
                     DataHora = item.DataHora
                    ,Disponivel = item.Disponivel
                    ,Id = item.Id
                    ,MedicoId = item.MedicoId
                };
                listHorarioDisponivelVO.Add(new HorarioDisponivelVO(horarioDisponivel, medico));
            } 
            
            return Ok(listHorarioDisponivelVO);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize(Roles = "Medico")]
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] HorarioDisponivel horario)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var novoId = await _horarioRepository.AdicionarAsync(horario);
        return CreatedAtAction(nameof(ObterPorMedico), new { medicoId = horario.MedicoId }, horario);
    }

    [Authorize(Roles = "Medico")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var removido = await _horarioRepository.RemoverAsync(id);
        if (!removido)
            return NotFound();

        return NoContent();
    }
}
