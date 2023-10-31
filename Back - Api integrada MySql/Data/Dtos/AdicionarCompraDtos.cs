using System.ComponentModel.DataAnnotations;

namespace Back___Api_integrada_MySql.Data.Dtos;

public class AdicionarCompraDtos
{
    [Required(ErrorMessage = "A quantidade deve ser obrigatória!")]
    public int Quantidade { get; set; }
    [Required(ErrorMessage = "O id do cliente deve ser obrigatório1")]
    public int ClienteId { get; set; }
    [Required(ErrorMessage = "o id do produto deve ser obrigatório!")]
    public int ProdutoId { get; set; }
}
