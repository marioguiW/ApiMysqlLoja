namespace Back___Api_integrada_MySql;

public class Compra
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }

}
