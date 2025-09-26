namespace ClasOhlsonCloneApi;

public class CartData
{
    public CartData(int id, int productId, int quantity, int userId, string name, decimal price)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        UserId = userId;
        Name = name;
        Price = price;
    }
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}