namespace ClasOhlsonCloneApi;

public class CartData
{
    public CartData(int id, int productId, int quantity, int userId)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        UserId = userId;
    }
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UserId { get; set; }
}