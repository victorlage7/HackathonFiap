using Microsoft.AspNetCore.Mvc;
using Health_Med.Services;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Health_Med.Model;
using Health_Med.VOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Health_Med.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IDbConnection _dbConnection;

        public AuthController(JwtService jwtService, IDbConnection dbConnection)
        {
            _jwtService = jwtService;
            _dbConnection = dbConnection;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.TipoUsuario == TipoUsuario.Medico)
            {
                var medico = await _dbConnection.QueryFirstOrDefaultAsync<Medico>(
                    "SELECT * FROM Medicos WHERE CRM = @Login AND Senha = @Senha",
                    new {Login = int.Parse(request.Login), request.Senha });

                if (medico == null)
                    return Unauthorized("Credenciais inválidas.");

                var token = _jwtService.GerarToken(medico.Id, "Medico");
                return Ok(new { Token = token });
            }
            else if (request.TipoUsuario == TipoUsuario.Paciente)
            {
                var paciente = await _dbConnection.QueryFirstOrDefaultAsync<Paciente>(
                    "SELECT * FROM Pacientes WHERE Email = @Login AND Senha = @Senha",
                    new {CPF = request.Login, request.Senha });

                if (paciente == null)
                    return Unauthorized("Credenciais inválidas.");

                var token = _jwtService.GerarToken(paciente.Id, "Paciente");
                return Ok(new { Token = token });
            }

            return BadRequest("Tipo de usuário inválido.");
        }
        [Authorize]
        [HttpGet("verificar")]
        public IActionResult VerificarAutenticacao()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return Unauthorized(new { Message = "Usuário não autenticado." });

            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var expirationClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            DateTime? expirationDate = null;
            if (expirationClaim != null)
            {
                var expUnix = long.Parse(expirationClaim);
                expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
            }

            return Ok(new
            {
                Message = "Usuário autenticado com sucesso!",
                UserId = userId,
                Role = role,
                TokenExpiraEm = expirationDate
            });
        }

    }
}
