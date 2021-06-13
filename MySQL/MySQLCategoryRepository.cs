using ClassLibraryApp.Interface;
using ClassLibraryApp.Table;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MySQL
{
    public class MySQLCategoryRepository : ICategoryRepository
    {
        protected string confStr { get; set; }

        public MySQLCategoryRepository()
        {
            confStr = "server=localhost;user=root;database=mydb;port=3306;password=1234";
        }

        public void Add(Category category)
        {
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("INSERT INTO mydb.categories(Name) " +
                    "VALUES (@categories.Name);", connection);

                command.Parameters.AddWithValue("@categories.Name", category.Name);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete(int categoryID)
        {
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("DELETE FROM mydb.categories WHERE ID = @categoryID;", connection);

                command.Parameters.AddWithValue("@categoryID", categoryID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Category[] GetAll()
        {
            List<Category> categories = new List<Category>();
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT * FROM mydb.categories;", connection);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category(reader.GetInt32(0), reader.GetString(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return categories.ToArray();
        }

        public Category GetCategory(int categoryID)
        {
            Category category = null;
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT * FROM mydb.categories WHERE ID = @categoryID;", connection);

                command.Parameters.AddWithValue("@categoryID", categoryID);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category = new Category(reader.GetInt32(0), reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return category;
        }

        public void Update(Category category)
        {
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();
                using var command = new MySqlCommand("UPDATE mydb.categories SET " +
                    "Name = @category.Name " +
                    "WHERE ID = @category.ID;", connection);

                command.Parameters.AddWithValue("@category.ID", category.ID);
                command.Parameters.AddWithValue("@category.Name", category.Name);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int GetCategoryID(string categoryName)
        {
            int categoryID = 0;
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT * FROM mydb.categories WHERE Name = @categoryName;", connection);

                command.Parameters.AddWithValue("@categoryName", categoryName);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categoryID = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return categoryID;
        }

        public double maxAvgCost(int idCity)
        {
            double max = 0;
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT MAX(avg_price) AS max_avg_price FROM (SELECT city.denomination AS city_hotel, hotel.name AS name_hotel, AVG(room.price) AS avg_price FROM hotel JOIN room ON room.idhotel = hotel.idhotel JOIN city ON hotel.idCity = city.idCity WHERE city.idCity = @idCity GROUP BY name_hotel) AS table1;", connection);

                command.Parameters.AddWithValue("@idCity", idCity);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    max = reader.GetDouble(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return max;
        }
    }
}
