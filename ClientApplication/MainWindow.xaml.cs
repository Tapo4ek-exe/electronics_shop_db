using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibraryApp.Interface;
using ClassLibraryApp.Table;
using MySQL;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IProductRepository ProductRepository = new MySQLProductRepository();
        public static ICategoryRepository CategoryRepository = new MySQLCategoryRepository();
        public int state = 1;

        public MainWindow()
        {
            InitializeComponent();
            dataGrid.MaxColumnWidth = 200;
            radioButton.IsChecked = true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)    // Удаление записи
        {
            if (state == 1)
            {
                Product product = (Product)dataGrid.SelectedItem;
                ProductRepository.Delete(product.ID);
                dataGrid.ItemsSource = ProductRepository.GetAll();
            }
            else
            {
                Category category = (Category)dataGrid.SelectedItem;

                // Проверка товаров удаляемой категории
                Product[] products = ProductRepository.GetAll();
                foreach(Product product in products)
                    if(product.Category.ID == category.ID)
                    {
                        MessageBox.Show("Данная категория используется товарами!");
                        return;
                    }
                
                CategoryRepository.Delete(category.ID);
                dataGrid.ItemsSource = CategoryRepository.GetAll();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)    // обновление информации о записи
        {
            try
            {
                if (state == 1)
                {
                    if (ProductIsEmpty())
                    {
                        MessageBox.Show("Введены не все поля");
                        return;
                    }

                    int id = ((Product)dataGrid.SelectedItem).ID;
                    Product product = new Product(id, textBox.Text, textBox2.Text, textBox3.Text, new Category(CategoryRepository.GetCategoryID(textBox1_Copy.Text), textBox1_Copy.Text));
                    ProductRepository.Update(product);
                    dataGrid.ItemsSource = ProductRepository.GetAll();
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(textBox.Text))
                    {
                        MessageBox.Show("Введены не все поля");
                        return;
                    }

                    int id = ((Category)dataGrid.SelectedItem).ID;
                    Category category = new Category(id, textBox.Text);
                    CategoryRepository.Update(category);
                    dataGrid.ItemsSource = CategoryRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)    // товары заданной компании
        {
            Product [] products = ProductRepository.allProductsOfCompany(textBox1.Text);
            dataGrid.ItemsSource = products;
        }

        private void button5_Click(object sender, RoutedEventArgs e)    // товары заданной категории
        {
            Product[] products = ProductRepository.allProductsInCategory(CategoryRepository.GetCategoryID(textBox4.Text));
            dataGrid.ItemsSource = products;
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)  // выбор базы товаров
        {
            state = 1;

            label3.Visibility = Visibility.Visible;
            textBox3.Visibility = Visibility.Visible;

            label4.Visibility = Visibility.Visible;
            textBox2.Visibility = Visibility.Visible;

            label2.Visibility = Visibility.Visible;
            textBox3.Visibility = Visibility.Visible;

            textBox1_Copy.Visibility = Visibility.Visible;

            StackPanel1.Visibility = Visibility.Visible;
            StackPanel2.Visibility = Visibility.Visible;

            textBox.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1_Copy.Clear();
            textBox4.Clear();

            Product[] products = ProductRepository.GetAll();
            dataGrid.ItemsSource = products;
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e) // выбор базы категорий
        {
            state = 2;

            label3.Visibility = Visibility.Hidden;
            textBox3.Visibility = Visibility.Hidden;

            label4.Visibility = Visibility.Hidden;
            textBox2.Visibility = Visibility.Hidden;

            label2.Visibility = Visibility.Hidden;
            textBox3.Visibility = Visibility.Hidden;

            textBox1_Copy.Visibility = Visibility.Hidden;

            StackPanel1.Visibility = Visibility.Hidden;
            StackPanel2.Visibility = Visibility.Hidden;

            textBox.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1_Copy.Clear();
            textBox4.Clear();

            Category[] categories = CategoryRepository.GetAll();
            dataGrid.ItemsSource = categories;
        }

        private void dataGrid_MouseUp(object sender, RoutedEventArgs e) // отображение информации о текущей записи
        {
            if (state == 1)
            {
                Product product = (Product)dataGrid.SelectedItem;
                textBox.Text = product.Name;
                textBox2.Text = product.Model;
                textBox3.Text = product.Description;
                textBox1_Copy.Text = product.Category.Name;
            }
            else
            {
                Category category = (Category)dataGrid.SelectedItem;
                textBox.Text = category.Name;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e) // создание новой записи
        {
            if (state == 1)
            {
                if(ProductIsEmpty())
                {
                    MessageBox.Show("Введены не все поля");
                    return;
                }

                Product product = new Product(0, textBox.Text, textBox2.Text, textBox3.Text, new Category(CategoryRepository.GetCategoryID(textBox1_Copy.Text), textBox1_Copy.Text));
                ProductRepository.Add(product);
                dataGrid.ItemsSource = ProductRepository.GetAll();
            }
            else
            {
                if (String.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show("Введены не все поля");
                    return;
                }

                Category category = new Category(0, textBox.Text);
                CategoryRepository.Add(category);
                dataGrid.ItemsSource = CategoryRepository.GetAll();
            }
            dataGrid.SelectedItem = dataGrid.Items[dataGrid.Items.Count - 1];
        }

        private bool ProductIsEmpty()
        {
            return String.IsNullOrWhiteSpace(textBox.Text) ||
                    String.IsNullOrWhiteSpace(textBox2.Text) || 
                    String.IsNullOrWhiteSpace(textBox1_Copy.Text) || 
                    (CategoryRepository.GetCategoryID(textBox1_Copy.Text) == 0);
        }
    }
}
