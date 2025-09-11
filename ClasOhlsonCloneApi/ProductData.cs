namespace ClasOhlsonCloneApi;

public class ProductData
{
    public ProductData(int id , string name, int price, string category, string imageUrl )
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
        ImageUrl = imageUrl;
    }


    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
}