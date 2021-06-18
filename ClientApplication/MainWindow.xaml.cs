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
        public static IProductRepository ProductRepository = new MySQLProductRepository();      // база товаров
        public static ICategoryRepository CategoryRepository = new MySQLCategoryRepository();   // база категорий
        public int state = 1;   // выбранная база: 1 - товары, 2 - категории

        public MainWindow()
        {
            InitializeComponent();
            MinHeight = Height;
            MinWidth = Width;
            dataGrid.MaxColumnWidth = 200;
            ProductRadioButton.IsChecked = true;
            DescriptionTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void dataGrid_SizeChanged(object sender, RoutedEventArgs e)
        {
            if(!Double.IsNaN(dataGrid.ActualWidth))
                dataGrid.MaxColumnWidth = dataGrid.ActualWidth / 3;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)    // удаление записи
        {
            int oldIndex = dataGrid.SelectedIndex;      // сохранение текущего индекса таблицы
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

            // Смещение выбранной строки в таблице
            if (oldIndex < dataGrid.Items.Count)
                dataGrid.SelectedIndex = oldIndex;
            else if (dataGrid.Items.Count > 0)
                dataGrid.SelectedIndex = dataGrid.Items.Count - 1;

            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            dataGrid_MouseUp(this, new RoutedEventArgs());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)    // обновление информации о записи
        {
            try
            {
                if (state == 1)
                {
                    // Проверка полей
                    if (ProductIsEmpty())
                    {
                        MessageBox.Show("Введены не все поля");
                        return;
                    }

                    int id = ((Product)dataGrid.SelectedItem).ID;
                    Product product = new Product(id, NameTextBox.Text, ModelTextBox.Text, DescriptionTextBox.Text, new Category(CategoryRepository.GetCategoryID(CategoryСomboBox1.Text), CategoryСomboBox1.Text));
                    ProductRepository.Update(product);
                    dataGrid.ItemsSource = ProductRepository.GetAll();
                }
                else
                {
                    // Проверка имени категории
                    if (String.IsNullOrWhiteSpace(NameTextBox.Text))
                    {
                        MessageBox.Show("Введены не все поля");
                        return;
                    }

                    int id = ((Category)dataGrid.SelectedItem).ID;
                    Category category = new Category(id, NameTextBox.Text);
                    CategoryRepository.Update(category);
                    dataGrid.ItemsSource = CategoryRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowProductsOfCompany_Click(object sender, RoutedEventArgs e)    // товары заданной компании
        {
            Product [] products = ProductRepository.allProductsOfCompany(CompanyTextBox.Text);
            dataGrid.ItemsSource = products;
        }

        private void ShowProductsInCategory_Click(object sender, RoutedEventArgs e)    // товары заданной категории
        {
            if (CategoryСomboBox2.Text == "Все")
            {
                Product[] products = ProductRepository.GetAll();
                dataGrid.ItemsSource = products;
            }
            else
            {
                Product[] products = ProductRepository.allProductsInCategory(CategoryRepository.GetCategoryID(CategoryСomboBox2.Text));
                dataGrid.ItemsSource = products;
            }
        }

        private void ProductRadioButton_Checked(object sender, RoutedEventArgs e)  // выбор базы товаров
        {
            state = 1;
            
            // Настройка видимости элементов
            ModelLabel.Visibility = Visibility.Visible;
            ModelTextBox.Visibility = Visibility.Visible;

            DescriptionLabel.Visibility = Visibility.Visible;
            DescriptionTextBox.Visibility = Visibility.Visible;

            CategoryLabel1.Visibility = Visibility.Visible;
            CategoryСomboBox1.Visibility = Visibility.Visible;

            StackPanel1.Visibility = Visibility.Visible;
            StackPanel2.Visibility = Visibility.Visible;

            ClearButton_Click(this, new RoutedEventArgs());

            // Обновление таблицы
            Product[] products = ProductRepository.GetAll();
            dataGrid.ItemsSource = products;
            if (dataGrid.Items.Count > 0)
            {
                dataGrid.SelectedIndex = 0;
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                dataGrid_MouseUp(this, new RoutedEventArgs());
            }

            // Обновление списка категорий
            Category[] categories = CategoryRepository.GetAll();
            List <string> categoriesNames1 = new List<string>();
            List<string> categoriesNames2 = new List<string>();
            categoriesNames2.Add("Все");
            foreach (Category category in categories)
            {
                categoriesNames1.Add(category.Name);
                categoriesNames2.Add(category.Name);

            }
            CategoryСomboBox1.ItemsSource = categoriesNames1;
            CategoryСomboBox2.ItemsSource = categoriesNames2;
        }

        private void CategoryRadioButton_Checked(object sender, RoutedEventArgs e) // выбор базы категорий
        {
            state = 2;

            // Настройка видимости элементов
            ModelLabel.Visibility = Visibility.Hidden;
            ModelTextBox.Visibility = Visibility.Hidden;

            DescriptionLabel.Visibility = Visibility.Hidden;
            DescriptionTextBox.Visibility = Visibility.Hidden;

            CategoryLabel1.Visibility = Visibility.Hidden;
            CategoryСomboBox1.Visibility = Visibility.Hidden;

            StackPanel1.Visibility = Visibility.Hidden;
            StackPanel2.Visibility = Visibility.Hidden;

            ClearButton_Click(this, new RoutedEventArgs());

            // Обновление таблицы
            Category[] categories = CategoryRepository.GetAll();
            dataGrid.ItemsSource = categories;
            if (dataGrid.Items.Count > 0)
            {
                dataGrid.SelectedIndex = 0;
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                dataGrid_MouseUp(this, new RoutedEventArgs());
            }
        }

        private void dataGrid_MouseUp(object sender, RoutedEventArgs e) // отображение информации о текущей записи
        {
            if (state == 1)
            {
                Product product = (Product)dataGrid.SelectedItem;
                NameTextBox.Text = product.Name;
                ModelTextBox.Text = product.Model;
                DescriptionTextBox.Text = product.Description;
                CategoryСomboBox1.Text = product.Category.Name;
            }
            else
            {
                Category category = (Category)dataGrid.SelectedItem;
                NameTextBox.Text = category.Name;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) // создание новой записи
        {
            if (state == 1)
            {
                if(ProductIsEmpty())
                {
                    MessageBox.Show("Введены не все поля");
                    return;
                }

                int oldIndex = dataGrid.SelectedIndex;
                Product product = new Product(0, NameTextBox.Text, ModelTextBox.Text, DescriptionTextBox.Text, new Category(CategoryRepository.GetCategoryID(CategoryСomboBox1.Text), CategoryСomboBox1.Text));
                ProductRepository.Add(product);
                dataGrid.ItemsSource = ProductRepository.GetAll();  
            }
            else
            {
                if (String.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Введены не все поля");
                    return;
                }

                Category category = new Category(0, NameTextBox.Text);
                CategoryRepository.Add(category);
                dataGrid.ItemsSource = CategoryRepository.GetAll();
            }

            dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
            dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
            dataGrid_MouseUp(this, new RoutedEventArgs());
        }

        private bool ProductIsEmpty()   // проверка полей инормации о товаре
        {
            return String.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    String.IsNullOrWhiteSpace(ModelTextBox.Text) || 
                    String.IsNullOrWhiteSpace(CategoryСomboBox1.Text) || 
                    (CategoryRepository.GetCategoryID(CategoryСomboBox1.Text) == 0);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)    // очистка полей
        {
            NameTextBox.Clear();
            ModelTextBox.Clear();
            DescriptionTextBox.Clear();
            CategoryСomboBox1.SelectedIndex = 0;
            CategoryСomboBox2.SelectedIndex = 0;
            CompanyTextBox.Clear();
        }
    }
}
