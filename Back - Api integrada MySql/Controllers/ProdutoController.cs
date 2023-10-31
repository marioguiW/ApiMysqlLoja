using Back___Api_integrada_MySql.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back___Api_integrada_MySql.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    [HttpGet]
    [Route("produtos")]
    public async Task<IActionResult> GetAllProdutos([FromServices] LojaContexto contexto)
    {
        var produtos = await contexto.Produtos.AsNoTracking().ToListAsync();

        if(produtos == null) return NotFound();

        return Ok(produtos);
    }

    [HttpGet]
    [Route("produtos/{id}")]
    public async Task<IActionResult> GetProdutoId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var produto = await contexto.Produtos
            .FirstOrDefaultAsync(produto => produto.Id == id);

        if (produto == null) return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    [Route("produtos")]
    public async Task<IActionResult> PostProduto([FromServices] LojaContexto contexto, [FromBody] Produto novoProduto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var produto = new Produto
        {
            Titulo = novoProduto.Titulo,
            Categoria = novoProduto.Categoria,
            Preco = novoProduto.Preco,
            UnidadeMedida = novoProduto.UnidadeMedida
        };

        await contexto.Produtos.AddAsync(produto);
        await contexto.SaveChangesAsync();
        return Created($"produtos/{novoProduto.Id}", produto);
    }

    [HttpPut]
    [Route("produtos/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        ,[FromRoute] int id, [FromBody] Produto produto)
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

        if(produtoASerRemovido == null) return NotFound();

        contexto.Produtos.Remove(produtoASerRemovido);
        contexto.SaveChangesAsync();

        return NoContent();
    }

}