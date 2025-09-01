using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;


namespace ClasOhlsonCloneApi;

[AllowAnonymous]
[ApiController]
// Alle routes starter nå med /users
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

        // foreach (var user in results)
        // {
        //     // Console.Wri   teLine(
        //     //     $"ID: {user.Id} {user.Firname} {user.Lastname} {user.Email} {user.Number} {user.Password}");
        // }

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

    [HttpPost("register")]
    public IActionResult AddUser([FromBody] PostData newUser)
    {
        Console.WriteLine(
            $"Mottatt fra frontend: Fornavn: {newUser.Firname}, {newUser.Lastname}, {newUser.Email}, {newUser.Number}, {newUser.Password}");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        newUser.Password = passwordHash;
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            var cmd = new MySqlCommand("""
                                       INSERT INTO users(firname, lastname, email, number, password)
                                       VALUES (@firname, @lastname, @email, @number, @password);
                                       """, connection);

            // cmd.Parameters.AddWithValue("@id", newUser.Id);
            cmd.Parameters.AddWithValue("@firname", newUser.Firname);
            cmd.Parameters.AddWithValue("@lastname", newUser.Lastname);
            cmd.Parameters.AddWithValue("@email", newUser.Email);
            cmd.Parameters.AddWithValue("@number", newUser.Number);
            cmd.Parameters.AddWithValue("@password", newUser.Password);

            cmd.ExecuteNonQuery();

            var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", connection);
            var newId = Convert.ToInt32(idCmd.ExecuteScalar());
            newUser.Id = newId;
        }

        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult GetUserInfo([FromBody] LoginData loginInfo)
    {
        Console.WriteLine($"Fra front: {loginInfo.Email}");
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        var query = new MySqlCommand("SELECT * FROM users WHERE email = @email", connection);
        query.Parameters.AddWithValue("@email", loginInfo.Email);

        using var reader = query.ExecuteReader();
        
            if (reader.Read()) // Sjekker om vi fant bruker
            {
                var hashFromDb = reader.GetString(5);
                Console.WriteLine($"Hash fra DB: {hashFromDb}");
                var isValid = BCrypt.Net.BCrypt.Verify(loginInfo.Password, hashFromDb);
        
                if (isValid)
                {
                    return Ok(new { message = "Login successful", email = loginInfo.Email });
                }
                else
                {
                    return Unauthorized(new { message = "Wrong password" });
                }
            }
            else
            {
                return Unauthorized(new { message = "User not found" });
            }
    }
    }
