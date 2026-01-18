using MySql.Data.MySqlClient;

/* 
 DBManager — Simple MySQL helper.
 - Manages a shared `MySqlConnection` and opens it with `SetMySQLDBConnection()`.
 - Executes SELECT queries with `GetQuery(query)` and returns results as a List<string>.
 - Writes error/info messages to the console.
*/

namespace PageObjectModelPW.utilities
{
    class DBManager
    {


        public static MySqlConnection connection = null;


        public static void SetMySQLDBConnection( )
        {
            string connectionString = "server=127.0.0.1;user=root;password=selenium;database=seleniumdba";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("MySQL database connection established.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error establishing MySQL database connection: " + ex.Message);
            }


        }




        public static List<string> GetQuery(string query)
        {
            List<string> results = new List<string>();

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Assuming the SELECT statement returns a single string column
                        string value = reader.GetString(0);
                        results.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SELECT statement: " + ex.Message);
            }

            return results;
        }

    }
}
