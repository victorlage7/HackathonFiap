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

            HorarioDisponivel horarioDisponivel = new HorarioDisponivel()
            {
                
            };
            HorarioDisponivelVO horarioDisponivelVO = new HorarioDisponivelVO(horarioDisponivel, medico);
            return Ok(horarioDisponivelVO);
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
