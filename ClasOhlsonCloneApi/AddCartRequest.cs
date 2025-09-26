namespace ClasOhlsonCloneApi;

public class AddCartRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UserId { get; set; }
}