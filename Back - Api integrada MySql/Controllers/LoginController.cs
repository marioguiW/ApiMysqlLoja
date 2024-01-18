using Back___Api_integrada_MySql.Data;
using Back___Api_integrada_MySql.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                user = cliente,
                acessToken = token,
                refreshToken = refreshToken,
            };
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

            Console.WriteLine("Esquisito :", clientToChangeTokens.RefreshToken);
            Console.WriteLine("Esquisito 2: ", tokens.refreshToken);

            if (clientToChangeTokens.RefreshToken != tokens.refreshToken)
            {
                await Console.Out.WriteLineAsync(clientToChangeTokens.RefreshToken);
                await Console.Out.WriteLineAsync(tokens.refreshToken);
                Console.WriteLine("erro né pai tlg");

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
