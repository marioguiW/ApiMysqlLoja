namespace Back___Api_integrada_MySql;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public Boolean isAdmin { get; set; }
    public int EnderecoId { get; set; }
    public Endereco EnderecoDeEntrega { get; set; }
}
