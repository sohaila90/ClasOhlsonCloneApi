using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Mysqlx.Expect;

namespace ClasOhlsonCloneApi;
[AllowAnonymous]
[ApiController]

[Route("[controller]")]
public class CartController : ControllerBase
{
   private readonly string _connectionString;

   public CartController(IConfiguration configuration)
   {
      var mysqlPassword = configuration.GetValue<string>("MYSQL_PASSWORD");
      if (mysqlPassword == null)
      {
         throw new InvalidOperationException("\nThe environment variable 'MYSQL_PASSWORD' is not set.");
      }

      _connectionString = $"server=localhost;database_test_schema;user=apiuser;password={mysqlPassword};";
   }
   
    [HttpPost("add")]
    public IActionResult AddProduct()
    {
       using (var connection = new MySqlConnection(_connectionString))
       {
          connection.Open();
       }
       // var addItems = new List<CartData>();
       return Ok();
    }
}