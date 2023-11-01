using Back___Api_integrada_MySql.Data;
using Back___Api_integrada_MySql.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back___Api_integrada_MySql.Controllers;

[ApiController]
[Route("[controller]")]
public class CompraController : ControllerBase
{
    [HttpGet]
    [Route("compras")]
    public async Task<IActionResult> GetAllCompras([FromServices] LojaContexto contexto)
    {
        var compras = await contexto.Compras
            .Include(compra => compra.Produto)
            .Include(compra => compra.Cliente)
                .ThenInclude(cliente => cliente.EnderecoDeEntrega)
            .AsNoTracking()
            .ToListAsync();

        if (compras == null) return NotFound();

        return Ok(compras);
    }

    [HttpGet]
    [Route("compras/{id}")]
    public async Task<IActionResult> GetCompraId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var compra = await contexto.Compras
            .Include(compra => compra.Produto)
            .Include(compra => compra.Cliente)
                .ThenInclude(cliente => cliente.EnderecoDeEntrega)
            .FirstOrDefaultAsync(compra => compra.Id == id);

        if (compra == null) return NotFound();


        return Ok(compra);
    }

    [HttpPost]
    [Route("compras")]
    public async Task<IActionResult> PostCompra([FromServices] LojaContexto contexto
        ,[FromBody] AdicionarCompraDtos novaCompra)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var compra = new Compra
        {
            Quantidade = novaCompra.Quantidade,
            ClienteId = novaCompra.ClienteId,
            ProdutoId = novaCompra.ProdutoId,
        };

        var validaCliente = await contexto.Clientes
            .FirstOrDefaultAsync(cliente => cliente.Id == novaCompra.ClienteId);

        if (validaCliente == null) return BadRequest();

        var validaProduto = await contexto.Produtos
            .FirstOrDefaultAsync(produto => produto.Id == novaCompra.ProdutoId);

        if (validaProduto == null) return BadRequest();

        await contexto.Compras.AddAsync(compra);
        await contexto.SaveChangesAsync();

        var compraIncludes = await contexto.Compras
            .Include(compraBanco => compraBanco.Produto)
            .Include(compraBanco => compraBanco.Cliente)
                .ThenInclude(produto => produto.EnderecoDeEntrega)
            .FirstOrDefaultAsync(clienteid => clienteid.Id == compra.Id);


        return Created($"compras/{compra.Id}", compraIncludes);
    }

    [HttpPut]
    [Route("compras/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        , [FromRoute] int id, [FromBody] AdicionarCompraDtos compra)
    {
        var compraASerAtualizado = await contexto.Compras
            .FirstOrDefaultAsync(compra => compra.Id == id);

        if (compraASerAtualizado == null) return NotFound();

        var validaProduto = await contexto.Produtos
            .FirstOrDefaultAsync(produtos => produtos.Id == compra.ProdutoId);
        
        if (validaProduto == null) return NotFound();

        var validaCliente = await contexto.Clientes
            .FirstOrDefaultAsync(clientes => clientes.Id == compra.ProdutoId);

        if (validaCliente == null) return NotFound();

        compraASerAtualizado.Quantidade = compra.Quantidade;
        compraASerAtualizado.ProdutoId = compra.ProdutoId;
        compraASerAtualizado.ClienteId = compra.ClienteId;

        contexto.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("compras/{id}")]

    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id, [FromServices] LojaContexto contexto)
    {
        var compraASerRemovida = await contexto
            .Compras.FirstOrDefaultAsync(compra => compra.Id == id);

        if (compraASerRemovida == null) return NotFound();

        contexto.Compras.Remove(compraASerRemovida);
        contexto.SaveChangesAsync();

        return NoContent();
    }


}
