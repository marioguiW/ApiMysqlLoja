using Back___Api_integrada_MySql.Data;
using Back___Api_integrada_MySql.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Back___Api_integrada_MySql.Controllers
{
    [ApiController]
    [Route("v1")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task <ActionResult<dynamic>> Authenticate([FromBody] Login user
            , [FromServices] LojaContexto contexto)
        {
            var cliente = await contexto.Clientes
                .FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email && x.Senha == user.Senha);

            if(cliente == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            var token = TokenService.GenerateToken(cliente);
            var refreshToken = TokenService.GenerateRefreshToken();

            cliente.AcessToken = token;
            cliente.RefreshToken = refreshToken;

            contexto.SaveChangesAsync();
            

            cliente.Senha = "";

            return new
            {
                user = cliente
            };
        }

        [HttpPost]
        [Route("session")]
        public async Task<IActionResult> Session(string accessToken)
        {
            try
            {
                var principal = TokenService.GetPrincipalFromExpiredToken(accessToken);

                // Verifica se o principal contém a claim de expiração
                var expirationDateClaim = principal.FindFirst(JwtRegisteredClaimNames.Exp);
                if (expirationDateClaim == null)
                {
                    return Unauthorized("Token inválido - data de expiração ausente");
                }

                // Obtém a data de expiração do token
                var expirationDateUnix = long.Parse(expirationDateClaim.Value);
                var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationDateUnix).UtcDateTime;

                // Verifica se o token está expirado
                if (expirationDateTime <= DateTime.UtcNow)
                {
                    return Unauthorized("Token expirado");
                }

                // Exemplo: Obtendo o nome do usuário do token
                var userName = principal.FindFirst(ClaimTypes.Name)?.Value;

                // Faça o que precisa com as informações do token

                return Ok("Token válido");
            }
            catch (SecurityTokenException ex)
            {
                // Se houver uma exceção, o token é inválido
                return Unauthorized("Token inválido");
            }
        }

        public class Tokens
        {
            public string acessToken { get; set; }
            public string refreshToken { get; set; }
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Tokens tokens, [FromServices] LojaContexto contexto)
        {
            var principal = TokenService.GetPrincipalFromExpiredToken(tokens.acessToken);
            var username = principal.Identity.Name;

            var clientToChangeTokens = await contexto.Clientes.FirstOrDefaultAsync(x => x.AcessToken == tokens.acessToken);

            if (clientToChangeTokens.RefreshToken != tokens.refreshToken)
            {
                return new ObjectResult(new
                {
                    Error = "Refresh Token inválido"
                });
            }

            var newJwtToken = TokenService.GenerateToken(principal.Claims);
            var newJwtRefresh = TokenService.GenerateRefreshToken();

            clientToChangeTokens.AcessToken = newJwtToken;
            clientToChangeTokens.RefreshToken = newJwtRefresh;
            contexto.SaveChangesAsync();

            return new ObjectResult(new
            {
                acessToken = newJwtToken,
                refreshToken = newJwtRefresh
            });
        }
    }

}
