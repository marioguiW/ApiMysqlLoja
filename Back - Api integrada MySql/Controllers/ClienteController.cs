using Back___Api_integrada_MySql.Data;
using Back___Api_integrada_MySql.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back___Api_integrada_MySql.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    [HttpGet]
    [Route("clientes")]
    public async Task<IActionResult> GetAllClientes([FromServices] LojaContexto contexto)
    {
        var clientes = await contexto.Clientes.AsNoTracking().ToListAsync();

        if (clientes == null) return NotFound();

        return Ok(clientes);
    }

    [HttpGet]
    [Route("clientes/{id}")]
    public async Task<IActionResult> GetClienteId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var cliente = await contexto.Clientes
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (cliente == null) return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    [Route("clientes")]
    public async Task<IActionResult> PostCliente([FromServices] LojaContexto contexto
        ,[FromForm] AdicionarClienteDtos novoCliente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var cliente = new Cliente
        {
            Nome = novoCliente.Nome,
            EnderecoId = novoCliente.EnderecoId
        };

        await contexto.Clientes.AddAsync(cliente);
        await contexto.SaveChangesAsync();

        return Created($"clientes/{cliente.Id}", cliente);
    }

    [HttpPut]
    [Route("produtos/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        , [FromRoute] int id, [FromBody] Produto produto)
    {
        var produtoASerAtualizado = await contexto.Produtos
            .FirstOrDefaultAsync(produto => produto.Id == id);

        if (produtoASerAtualizado == null) return NotFound();

        produtoASerAtualizado.Titulo = produto.Titulo;
        produtoASerAtualizado.Preco = produto.Preco;
        produtoASerAtualizado.UnidadeMedida = produto.UnidadeMedida;
        produtoASerAtualizado.Categoria = produto.Categoria;

        contexto.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("produtos/{id}")]

    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id, [FromServices] LojaContexto contexto)
    {
        var produtoASerRemovido = await contexto
            .Produtos.FirstOrDefaultAsync(produto => produto.Id == id);

        if (produtoASerRemovido == null) return NotFound();

        contexto.Produtos.Remove(produtoASerRemovido);
        contexto.SaveChangesAsync();

        return NoContent();
    }

}
