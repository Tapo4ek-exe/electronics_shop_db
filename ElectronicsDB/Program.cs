using ClassLibraryApp.Interface;
using ClassLibraryApp.Table;
using Infrastructure.MySQL;
using System;

namespace ElectronicsDB
{

    class Program
    {
        public static IProductRepository ProductRepository = new MySQLProductRepository();
        public static ICategoryRepository CategoryRepository = new MySQLCategoryRepository();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("1) PrintProducts\n2) AddProduct\n3) DeleteProduct\n4) UpdateProduct\n5) PrintProduct\n6) AllProductsOfCompany\n7) AllProductsInCategory\n8) PrintCategories\n9) AddCategory\n10) DeleteCategory\n11) UpdateCategory\n12) PrintCategory\n13) Exit\nВведите команду для её выполнения: ");
                int command = 0;
                try
                {
                    command = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                switch (command)
                {
                    case 1:
                        PrintProducts();
                        break;
                    case 2:
                        AddProduct();
                        break;
                    case 3:
                        DeleteProduct();
                        break;
                    case 4:
                        UpdateProduct();
                        break;
                    case 5:
                        PrintProduct();
                        break;
                    case 6:
                        AllProductsOfCompany();
                        break;
                    case 7:
                        AllProductsInCategory();
                        break;
                    case 8:
                        PrintCategories();
                        break;
                    case 9:
                        AddCategory();
                        break;
                    case 10:
                        DeleteCategory();
                        break;
                    case 11:
                        UpdateCategory();
                        break;
                    case 12:
                        PrintCategory();
                        break;
                    case 13:
                        return;
                }
            }
        }

        public static void PrintProducts()
        {
            Product[] products = ProductRepository.GetAll();
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }
        public static void AddProduct()
        {
            Console.Write("Введите название нового товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите модель нового товара: ");
            string model = Console.ReadLine();

            Console.Write("Введите описание нового товара: ");
            string description = Console.ReadLine();

            Console.Write("Введите категорию нового товара: ");
            string category = Console.ReadLine();
            
            ProductRepository.Add(new Product(0, name, model, description, new Category(CategoryRepository.GetCategoryID(category), category)));

            Console.WriteLine("Добавлен новый товар");
        }

        public static void DeleteProduct()
        {
            Console.Write("Введите ID товара, который нужно удалить: ");
            int productID = Convert.ToInt32(Console.ReadLine());

            ProductRepository.Delete(productID);

            Console.WriteLine("Запись о товаре удалена");
        }

        public static void UpdateProduct()
        {
            Console.Write("Введите ID товара, который нужно обновить: ");
            int productID = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите новое название товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите новую модель товара: ");
            string model = Console.ReadLine();

            Console.Write("Введите новое описание товара: ");
            string description = Console.ReadLine();

            Console.Write("Введите новое название категории товара: ");
            string category = Console.ReadLine();

            
            ProductRepository.Update(new Product(productID, name, model, description, new Category(CategoryRepository.GetCategoryID(category), category)));

            Console.WriteLine("Выбранный товар обновлен");
        }

        public static void PrintProduct()
        {
            Console.Write("Введите ID товара, который нужно вывести: ");
            int productID = Convert.ToInt32(Console.ReadLine());

            var product = ProductRepository.GetProduct(productID);

            Console.WriteLine(product);
        }

        public static void AllProductsOfCompany()
        {
            Console.Write("Введите название компании: ");
            string companyName = Console.ReadLine();

            Product[] products = ProductRepository.allProductsOfCompany(companyName);

            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        public static void AllProductsInCategory()
        {
            Console.Write("Введите название категории: ");
            string categoryName = Console.ReadLine();

            Product[] products = ProductRepository.allProductsInCategory(CategoryRepository.GetCategoryID(categoryName));

            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        public static void PrintCategories()
        {
            Category[] categories = CategoryRepository.GetAll();         
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
        }
        public static void AddCategory()
        {
            Console.Write("Введите название новой категории: ");
            string name = Console.ReadLine();

            CategoryRepository.Add(new Category(0, name));

            Console.WriteLine("Добавленна новая категория");
        }

        public static void DeleteCategory()
        {
            Console.Write("Введите ID категории, которую нужно удалить: ");
            int categoryID = Convert.ToInt32(Console.ReadLine());

            CategoryRepository.Delete(categoryID);

            Console.WriteLine("Запись о категории удалена");
        }

        public static void UpdateCategory()
        {
            Console.Write("Введите ID категории, которую нужно обновить: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите новое название категории: ");
            string name = Console.ReadLine();

            CategoryRepository.Update(new Category(id, name));

            Console.WriteLine("Выбранная категория обновлена");
        }

        public static void PrintCategory()
        {
            Console.Write("Введите ID категории, которую нужно вывести: ");

            int categoryID = Convert.ToInt32(Console.ReadLine());

            var category = CategoryRepository.GetCategory(categoryID);

            Console.WriteLine(category);
        }
    }
}
