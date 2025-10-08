using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

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

      _connectionString = $"server=localhost;database=test_schema;user=apiuser;password={mysqlPassword};";
   }

   [HttpPost("add")]
   public IActionResult AddProduct([FromBody] AddCartRequest? cartItem)
   {
      if (cartItem == null)
         return BadRequest("Invalid payload");

      try
      {
         using (var connection = new MySqlConnection(_connectionString))
         {
            connection.Open();
            
            var cmd = new MySqlCommand("""
                                       INSERT INTO cart_items(product_id, quantity, user_id)
                                       VALUES(@product_id, @quantity, @user_id) 
                                       ON DUPLICATE KEY UPDATE quantity = quantity + @quantity
                                       """, connection);
            cmd.Parameters.AddWithValue("@product_id", cartItem.ProductId);
            cmd.Parameters.AddWithValue("@quantity", cartItem.Quantity);
            cmd.Parameters.AddWithValue("@user_id", cartItem.UserId);

            cmd.ExecuteNonQuery();
         }

         return Ok();
      }
      catch (Exception ex)
      {
         return StatusCode(500, "Kunne ikke legge til i handlekurv");
      }
   }

   [HttpGet]
   public IActionResult GetCartItems()
   {
      var itemsFromCart = new List<CartData>();
      using (var connection = new MySqlConnection(_connectionString))

      {
         connection.Open();
         var query = new MySqlCommand(
            @"SELECT c.id, c.product_id, c.quantity, c.user_id, p.name, p.price FROM cart_items c JOIN products p ON c.product_id = p.id",
            connection);

         using (var reader = query.ExecuteReader())
         {
            while (reader.Read())
            {
               try
               {
                  // håndterer NULL ved å gi default 0
                  int id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id");
                  int productId = reader.IsDBNull(reader.GetOrdinal("product_id")) ? 0 : reader.GetInt32("product_id");
                  int quantity = reader.IsDBNull(reader.GetOrdinal("quantity")) ? 0 : reader.GetInt32("quantity");
                  int userId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32("user_id");

                  string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name");
                  decimal price = reader.IsDBNull(reader.GetOrdinal("price")) ? 0 : reader.GetDecimal("price");

                  itemsFromCart.Add(new CartData(id, productId, quantity, userId, name, price));
               }
               catch (Exception ex)
               {
                  Console.WriteLine($"Feil i rad: {ex.Message}");
                  throw;
               }
            }
         }
      }

      return Ok(itemsFromCart);

   }



   [HttpDelete("remove")]
   public IActionResult RemoveProduct([FromBody] RemoveCartRequest? request)
   {
      if (request == null)
      {
         return BadRequest("Invalid payload");
      }
      
      try
      {
         using (var connection = new MySqlConnection(_connectionString))
         {
            connection.Open();

            var cmd = new MySqlCommand("""
                                           DELETE FROM cart_items
                                           WHERE product_id = @product_id AND user_id = @user_id;
                                       """, connection);

            cmd.Parameters.AddWithValue("product_id", request.ProductId);
            cmd.Parameters.AddWithValue("user_id", request.UserId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
               return NotFound("Product not found in cart");

            return Ok("Product removed");
         }
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Feil ved sletting: {ex}");
         return StatusCode(500, "Kunne ikke slette produkt");
      }
   }
}
   
      
       