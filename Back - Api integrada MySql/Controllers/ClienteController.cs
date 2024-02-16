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

        var clientesInclude = await contexto.Clientes
            .Include(cliente => cliente.EnderecoDeEntrega).AsNoTracking().ToListAsync();

        return Ok(clientesInclude);
    }

    [HttpGet]
    [Route("clientes/{id}")]
    public async Task<IActionResult> GetClienteId(
        [FromServices] LojaContexto contexto, [FromRoute] int id)
    {
        var cliente = await contexto.Clientes
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (cliente == null) return NotFound();

        var clienteInclude = await contexto.Clientes
            .Include(cliente => cliente.EnderecoDeEntrega)
            .FirstOrDefaultAsync(clienteBanco => clienteBanco.Id == cliente.Id);

        return Ok(clienteInclude);
    }

    [HttpPost]
    [Route("clientes")]
    public async Task<IActionResult> PostCliente([FromServices] LojaContexto contexto
        ,[FromBody] AdicionarClienteDtos novoCliente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var endereco = new Endereco
        {
            Cep = novoCliente.Endereco.Cep,
            Logradouro = novoCliente.Endereco.Logradouro,
            Bairro = novoCliente.Endereco.Bairro,
            Cidade = novoCliente.Endereco.Cidade,
            Numero = novoCliente.Endereco.Cep,
        };


        var cliente = new Cliente
        {
            Nome = novoCliente.Nome,
            Email = novoCliente.Email,
            Senha = novoCliente.Senha,
            isAdmin = false,
            AccessToken = null,
            RefreshToken = null
        };

        var verificaEndereco = await contexto
            .Enderecos
            .FirstOrDefaultAsync(endereco => endereco.Id == cliente.EnderecoId);

        Console.WriteLine(verificaEndereco);

        if (verificaEndereco == null)
        {
            return NotFound();
        }

        await contexto.Clientes.AddAsync(cliente);
        await contexto.SaveChangesAsync();

        

        var clienteInclude = await contexto.Clientes
            .Include(cliente => cliente.EnderecoDeEntrega)
            .FirstOrDefaultAsync(clienteBanco => clienteBanco.Id == cliente.Id);
        
        return Created($"clientes/{cliente.Id}", clienteInclude);
    }

    [HttpPut]
    [Route("clientes/{id}")]

    public async Task<IActionResult> UpdateAsync(
        [FromServices] LojaContexto contexto
        , [FromRoute] int id, [FromBody] AdicionarClienteDtos cliente)
    {
        var clienteASerAtualizado = await contexto.Clientes
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (clienteASerAtualizado == null) return NotFound();

        var validaEndereco = await contexto.Enderecos
            .FirstOrDefaultAsync (endereco => endereco.Id == cliente.EnderecoId);

        if(validaEndereco == null) return NotFound();

        clienteASerAtualizado.Nome = cliente.Nome;
        clienteASerAtualizado.Email = cliente.Email;
        clienteASerAtualizado.Senha = cliente.Senha;
        clienteASerAtualizado.EnderecoId = cliente.EnderecoId;

        contexto.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("clientes/{id}")]

    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id, [FromServices] LojaContexto contexto)
    {
        var clienteASerRemovido = await contexto
            .Clientes.FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (clienteASerRemovido == null) return NotFound();

        contexto.Clientes.Remove(clienteASerRemovido);
        contexto.SaveChangesAsync();

        return NoContent();
    }

}
