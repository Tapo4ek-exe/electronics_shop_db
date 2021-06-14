using ClassLibraryApp.Interface;
using ClassLibraryApp.Table;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL
{
    public class MySQLProductRepository : IProductRepository
    {
        protected string confStr { get; set; }

        public MySQLProductRepository()
        {
            confStr = "server=localhost;user=root;database=mydb;port=3306;password=1234";
        }

        
        public void Add(Product product)
        {
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("INSERT INTO mydb.products(products.Name, products.Model, products.Description, products.Category_ID) " +
                    "VALUES (@product.Name, @product.Model, @product.Description, @product.Category_ID);", connection);

                command.Parameters.AddWithValue("@product.Name", product.Name);
                command.Parameters.AddWithValue("@product.Model", product.Model);
                command.Parameters.AddWithValue("@product.Description", product.Description);
                command.Parameters.AddWithValue("@product.Category_ID", product.Category.ID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete(int productID)
        {           
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("DELETE FROM mydb.products WHERE ID = @productID;", connection);

                command.Parameters.AddWithValue("@productID", productID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        public Product[] GetAll()
        {
            List<Product> products = new List<Product>();
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT products.ID, products.Name, products.Model, products.Description, products.Category_ID, categories.Name FROM mydb.products JOIN mydb.categories ON products.Category_ID = categories.ID ORDER BY products.ID;", connection);

                using var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    if(!reader.IsDBNull(3))
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), new Category(reader.GetInt32(4), reader.GetString(5))));
                    else
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), "", new Category(reader.GetInt32(4), reader.GetString(5))));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products.ToArray();
        }

        public Product GetProduct(int productID)
        {
            Product product = null;
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT products.ID, products.Name, products.Model, products.Description, products.Category_ID, categories.Name FROM mydb.products JOIN mydb.categories ON products.Category_ID = categories.ID WHERE products.ID = @productID;", connection);

                command.Parameters.AddWithValue("@productID", productID);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    product = new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), new Category(reader.GetInt32(4), reader.GetString(5)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return product;
        }

        public void Update(Product product)
        {
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("UPDATE mydb.products SET " +
                    "products.Name = @product.Name, " +
                    "products.Model = @product.Model, " +
                    "products.Description = @product.Description, " +
                    "products.Category_ID = @product.Category_ID " +
                    "WHERE products.ID = @product.ID;", connection);

                command.Parameters.AddWithValue("@product.ID", product.ID);
                command.Parameters.AddWithValue("@product.Name", product.Name);
                command.Parameters.AddWithValue("@product.Model", product.Model);
                command.Parameters.AddWithValue("@product.Description", product.Description);
                command.Parameters.AddWithValue("@product.Category_ID", product.Category.ID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Product[] allProductsInCategory(int categoryID)
        {
            List<Product> products = new List<Product>();
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT products.ID, products.Name, products.Model, products.Description, products.Category_ID, categories.Name FROM mydb.products JOIN mydb.categories ON products.Category_ID = categories.ID WHERE products.Category_ID = @categoryID;", connection);

                command.Parameters.AddWithValue("@categoryID", categoryID);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(3))
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), new Category(reader.GetInt32(4), reader.GetString(5))));
                    else
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), "", new Category(reader.GetInt32(4), reader.GetString(5))));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products.ToArray();
        }

        public Product[] allProductsOfCompany(string companyName)
        {
            List<Product> products = new List<Product>();
            try
            {
                using var connection = new MySqlConnection(confStr);

                connection.Open();

                using var command = new MySqlCommand("SELECT products.ID, products.Name, products.Model, products.Description, products.Category_ID, categories.Name FROM mydb.products JOIN mydb.categories ON products.Category_ID = categories.ID WHERE products.Name LIKE @companyName;", connection);

                companyName = companyName.Insert(0, "%"); companyName = companyName.Insert(companyName.Length, "%");
                command.Parameters.AddWithValue("@companyName", companyName);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(3))
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), new Category(reader.GetInt32(4), reader.GetString(5))));
                    else
                        products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), "", new Category(reader.GetInt32(4), reader.GetString(5))));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products.ToArray();
        }
    }
}
