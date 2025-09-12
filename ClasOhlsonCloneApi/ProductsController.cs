using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace ClasOhlsonCloneApi;

[AllowAnonymous]
[ApiController]

[Route("[controller]")]

public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly string _connectionString;

    public ProductsController(IConfiguration configuration, ILogger<ProductsController> logger)
    {
        var mysqlPassword = configuration.GetValue<string>("MYSQL_PASSWORD");
        if (mysqlPassword == null)
        {
            throw new InvalidOperationException("\nThe environment variable 'MYSQL_PASSWORD' is not set");
        }

        _connectionString = $"server=localhost; database=test_schema;user=apiuser;password={mysqlPassword};";
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetProducts() 
    {
        // list to add our products from the db
        var products = new List<ProductData>();
        _logger.LogInformation("GetProducts() is called!");
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query = new MySqlCommand("SELECT * FROM products", connection);
            
            using (var reader = query.ExecuteReader())
            {
                _logger.LogInformation("Query running!");
                while (reader.Read())
                {
                    _logger.LogInformation("Found product: {Name}", reader.GetString("name"));
                    products.Add(new ProductData(
                        reader.GetInt32("id"),
                        reader.GetString("name"),
                        reader.GetInt32("price"),
                        reader.GetString("category"),
                        reader.GetString("image_url")
                    ));
                }
            }
        }

        return Ok(products);

        }
        
    }
