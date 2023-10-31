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
        var compras = await contexto.Compras.AsNoTracking().ToListAsync();

        if (compras == null) return NotFound();

        return Ok(compras);
    }

    [HttpGet]
    [Route("compras/{id}")]
    public async Task<IActionResult> GetProdutoId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var compra = await contexto.Compras
            .FirstOrDefaultAsync(compra => compra.Id == id);

        if (compra == null) return NotFound();

        return Ok(compra);
    }

    [HttpPost]
    [Route("compras")]
    public async Task<IActionResult> PostProduto([FromServices] LojaContexto contexto
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

        await contexto.Compras.AddAsync(compra);
        await contexto.SaveChangesAsync();
        return Created($"compras/{compra.Id}", compra);
    }

    [HttpPut]
    [Route("compras/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        , [FromRoute] int id, [FromBody] Compra compra)
    {
        var compraASerAtualizado = await contexto.Compras
            .FirstOrDefaultAsync(compra => compra.Id == id);

        if (compraASerAtualizado == null) return NotFound();

        compraASerAtualizado.Quantidade = compra.Quantidade;
        compraASerAtualizado.ProdutoId = compra.ProdutoId;
        compraASerAtualizado.ClienteId = compra.ClienteId;


        contexto.SaveChangesAsync();

        return NoContent();
    }

    //[HttpDelete]
    //[Route("produtos/{id}")]

    //public async Task<IActionResult> DeleteAsync(
    //    [FromRoute] int id, [FromServices] LojaContexto contexto)
    //{
    //    var produtoASerRemovido = await contexto
    //        .Produtos.FirstOrDefaultAsync(produto => produto.Id == id);

    //    if (produtoASerRemovido == null) return NotFound();

    //    contexto.Produtos.Remove(produtoASerRemovido);
    //    contexto.SaveChangesAsync();

    //    return NoContent();
    //}


}
