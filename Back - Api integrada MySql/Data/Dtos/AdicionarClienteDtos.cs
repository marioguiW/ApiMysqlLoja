using System.ComponentModel.DataAnnotations;

namespace Back___Api_integrada_MySql.Data.Dtos;

public class AdicionarClienteDtos
{
    [Required(ErrorMessage = "O Nome do cliente deve ser obrigatório!")]
    public string Nome {  get; set; }
    [Required(ErrorMessage = "O email deve ser obrigatório")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Senha deve ser obrigatório")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O id de endereço deve ser obrigatório!")]
    public int EnderecoId { get; set; }
}
