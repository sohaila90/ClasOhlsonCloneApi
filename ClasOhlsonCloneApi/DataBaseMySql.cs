using MySql.Data.MySqlClient;

namespace ClasOhlsonCloneApi
{
    public class DataBaseMySql
    {
        private MySqlConnection ConnectToDb()
        {
            try
            {
                const string connectionString =
                    "Server=localhost;Port=3306;Database=test_schema;User=root;Password=NyttSterktPassord123!;";
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                // Console.WriteLine("✅ Koblet til databasen!");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Feil: " + ex.Message);
            }

            return null!;
        }

        public object QueryDb(string query)
        {
            var connection = ConnectToDb();
            if (connection == null!) return null!;

            using var command = new MySqlCommand(query, connection);
            var result = command.ExecuteScalar();
            Console.WriteLine("Første rad i notes-tabellen: " + result);
            connection.Close();
            return result!;
        }
    }
}

