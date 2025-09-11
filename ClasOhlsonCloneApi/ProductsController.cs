using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace ClasOhlsonCloneApi;

[AllowAnonymous]
[ApiController]

[Route("[controller]")]

public class ProductsController : ControllerBase
{
    private string _connectionString;

    public ProductsController(IConfiguration configuration)
    {
        var mysqlPassword = configuration.GetValue<string>("MYSQL_PASSWORD");
        if (mysqlPassword == null)
        {
            throw new InvalidOperationException("\nThe environment variable 'MYSQL_PASSWORD' is not set");
        }

        _connectionString = $"server=localhost; database=test_schema;users=apiuser;password={mysqlPassword};";
    }

    [HttpGet]
    public IActionResult GetProducts() 
    {
        // list to add our products from the db
        var products = new List<ProductData>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query = new MySqlCommand("SELECT * FROM products", connection);
            using (var reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new ProductData(
                        reader.GetInt32("id"),
                        reader.GetString("name"),
                        reader.GetInt32("price"),
                        reader.GetString("category"),
                        reader.GetString("imageUrl")
                    ));
                }
            }
        }

        return Ok(products);

        }
        
    }
