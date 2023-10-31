using Back___Api_integrada_MySql.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back___Api_integrada_MySql.Controllers;

[ApiController]
[Route("[controller]")]
public class EnderecoController : ControllerBase
{
    [HttpGet]
    [Route("enderecos")]
    public async Task<IActionResult> GetAllEnderecos([FromServices] LojaContexto contexto)
    {
        var enderecos = await contexto.Enderecos.AsNoTracking().ToListAsync();

        if (enderecos == null) return NotFound();

        return Ok(enderecos);
    }

    [HttpGet]
    [Route("enderecos/{id}")]
    public async Task<IActionResult> GetEnderecoId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var endereco = await contexto.Enderecos
            .FirstOrDefaultAsync(endereco => endereco.Id == id);

        if (endereco == null) return NotFound();

        return Ok(endereco);
    }

    [HttpPost]
    [Route("enderecos")]
    public async Task<IActionResult> PostEndereco([FromServices] LojaContexto contexto, [FromBody] Endereco novoEndereco)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var endereco = new Endereco
        {
            Cep = novoEndereco.Cep,
            Logradouro = novoEndereco.Logradouro,
            Bairro = novoEndereco.Bairro,
            Cidade = novoEndereco.Cidade,
            Numero = novoEndereco.Numero,
        };

        await contexto.Enderecos.AddAsync(endereco);
        await contexto.SaveChangesAsync();

        return Created($"enderecos/{endereco.Id}", endereco);
    }

    [HttpPut]
    [Route("enderecos/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        , [FromRoute] int id, [FromBody] Endereco endereco)
    {
        var enderecoASerAtualizado = await contexto.Enderecos
            .FirstOrDefaultAsync(endereco => endereco.Id == id);

        if (enderecoASerAtualizado == null) return NotFound();

        enderecoASerAtualizado.Cep = endereco.Cep;
        enderecoASerAtualizado.Logradouro = endereco.Logradouro;
        enderecoASerAtualizado.Bairro = endereco.Bairro;
        enderecoASerAtualizado.Cidade = endereco.Cidade;
        enderecoASerAtualizado.Numero = endereco.Numero;

        contexto.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("enderecos/{id}")]

    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id, [FromServices] LojaContexto contexto)
    {
        var enderecoASerRemovido = await contexto
            .Enderecos.FirstOrDefaultAsync(endereco => endereco.Id == id);

        if (enderecoASerRemovido == null) return NotFound();

        contexto.Enderecos.Remove(enderecoASerRemovido);
        contexto.SaveChangesAsync();

        return NoContent();
    }

}
