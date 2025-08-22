
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;



namespace ClasOhlsonCloneApi
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration configuration)
        {
            var mysqlPassword = configuration.GetValue<string>("MYSQL_PASSWORD");

            if (mysqlPassword == null)
            {
                throw new InvalidOperationException("\nThe environment variable 'MYSQL_PASSWORD' is not set.");
            }
            _connectionString = $"server=localhost;database=test_schema;user=apiuser;password={mysqlPassword};";
        }

        
        [HttpGet]
        public IActionResult GetUsers()
        {
            var results = new List<PostData>();
            // var userPost = new List<PostData>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                 connection.Open();
                var query = new MySqlCommand("SELECT * FROM users", connection);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new PostData(

                            reader.GetInt32("id"),
                            reader.GetString("firname"),
                            reader.GetString("lastname"),
                            reader.GetString("email"),
                            reader.GetString("number"),
                            reader.GetString("password")
                        ));
                    }
                }



            }

            foreach (var user in results)
            {
                Console.WriteLine(
                    $"ID: {user.Id} {user.Firname} {user.Lastname} {user.Email} {user.Number} {user.Password}");
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            PostData? user = null;

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            var query = new MySqlCommand("SELECT * FROM users WHERE id = @id", connection);
            query.Parameters.AddWithValue("id", id);

            using (var reader = query.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new PostData(
                        reader.GetInt32("id"),
                        reader.GetString("firname"),
                        reader.GetString("lastname"),
                        reader.GetString("email"),
                        reader.GetString("number"),
                        reader.GetString("password")

                    );
                }
            }

            if (user == null)
                return NotFound(new { message = "User not found" });
            return Ok(user);
        }

        [HttpPost("/register")]
        public IActionResult AddUser([FromBody] PostData newUser)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(@"
                INSERT INTO users(id, firname, lastname, email, number, password)
                VALUES (@id, @firname, @lastname, @email, @number, @password, NOW());
                SELECT LAST _INSERT_ID();", connection);

                cmd.Parameters.AddWithValue("@id", newUser.Id);
                cmd.Parameters.AddWithValue("@firname", newUser.Firname);
                cmd.Parameters.AddWithValue("@lastname", newUser.Lastname);
                cmd.Parameters.AddWithValue("@email", newUser.Email);
                cmd.Parameters.AddWithValue("@number", newUser.Number);
                cmd.Parameters.AddWithValue("@password", newUser.Password);

                var newId = Convert.ToInt32(cmd.ExecuteScalar());
                newUser.Id = newId;
            }

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        // [HttpPost]
        // public IActionResult PostUsers( PostData postData)
        // {
        //     // results.Firname;
        //     // if (postData == null)
        //     // {
        //     //     return BadRequest();
        //     // }
        //     // var newUser = use
        //     return Ok(postData);
        // }

    }
}




// using Microsoft.AspNetCore.Mvc;
//
// namespace ClasOhlsonCloneApi
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class NotesController : ControllerBase
//     {
//         [HttpGet]
//         public IActionResult GetNote()
//         {
//             var db = new DataBaseMySql();
//             var result = db.QueryDb("SELECT textvalue FROM test_schema.new_data");
//             
//             if(result != null)
//             {
//                 return Ok(result.ToString());
//             }
//
//             return NotFound("Ingen data funnet");
//         }
//
//         [HttpPost]
//         public IActionResult PostNote(PostData postData)
//         {
//             return Ok(postData);
//         }
//     }
// }
//
//     
//     
