using Health_Med.Model;
using Health_Med.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoController(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        [Authorize(Roles = "Medico,Paciente")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var medico = await _medicoRepository.ObterPorIdAsync(id);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [Authorize(Roles = "Medico,Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var medicos = await _medicoRepository.ObterTodosAsync();
            return Ok(medicos);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Medico medico)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var novoId = await _medicoRepository.AdicionarAsync(medico);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoId }, medico);
        }

        [Authorize(Roles = "Medico")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Medico medico)
        {
            if (id != medico.Id)
                return BadRequest();

            var existe = await _medicoRepository.ObterPorIdAsync(id);
            if (existe == null)
                return NotFound();

            var atualizado = await _medicoRepository.AtualizarAsync(medico);
            if (!atualizado)
                return StatusCode(500, "Erro ao atualizar o médico.");

            return NoContent();
        }

        [Authorize(Roles = "Medico")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var existe = await _medicoRepository.ObterPorIdAsync(id);
            if (existe == null)
                return NotFound();

            var removido = await _medicoRepository.RemoverAsync(id);
            if (!removido)
                return StatusCode(500, "Erro ao remover o médico.");

            return NoContent();
        }

        [Authorize(Roles = "Medico,Paciente")]
        [HttpPost("{especialdiade}")]
        public async Task<IActionResult> BuscarPorEspecialdiade(int especialdiade)
        {
            var medicos = await _medicoRepository.ObterPorEspecialdiadeAsync(especialdiade);

            if (medicos == null)
                return NotFound();

            return Ok(medicos);
        }

    }
}
