using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.Json;
namespace InternetMagazin
{
    public partial class Form1 : Form
    {
        // Модель товара
        
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
            // Основные параметры для продука
        }
        // Модель корзины и элемента корзины
        public class CartItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }

            public decimal TotalPrice => Product.Price * Quantity;
        }
        //Модель заказа
        public class Order
        {
            public string OrderNumber { get; set; }
            public List<CartItem> Items { get; set; }
            public string CustomerName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Status { get; set; } // Новый, В обработке и т.д.
            public DateTime OrderDate { get; set; }
        }
        // модуль католог товара
        public class CatalogService
        {
            private const string ProductsFilePath = "products.json";

            public List<Product> LoadProducts()
            {
                try
                {
                    var json = File.ReadAllText(ProductsFilePath);
                    var products = JsonConvert.DeserializeObject<List<Product>>(json);
                    return products;
                }
                catch (Exception ex)
                {
                    // логика логирования
                    Console.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
                    return new List<Product>();
                }
                //Коммит: реализована загрузка и отображение товаров из JSON файла в каталоге.
            }
        }
        // модуль корзина
        public class ShoppingCart
        {
            public List<CartItem> Items { get; private set; } = new List<CartItem>();

            public void AddProduct(Product product)
            {
                var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    Items.Add(new CartItem { Product = product, Quantity = 1 });
                }
            }

            public void RemoveProduct(Product product)
            {
                var item = Items.FirstOrDefault(i => i.Product.Id == product.Id);
                if (item != null)
                {
                    Items.Remove(item);
                }
            }

            public void ChangeQuantity(Product product, int quantity)
            {
                var item = Items.FirstOrDefault(i => i.Product.Id == product.Id);
                if (item != null)
                {
                    item.Quantity = quantity;
                }
            }

            public decimal GetTotalSum()
            {
                return Items.Sum(i => i.TotalPrice);
            }

            public void SaveToFile()
            {
                try
                {
                    var json = JsonConvert.SerializeObject(Items);
                    File.WriteAllText("cart.json", json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения корзины: {ex.Message}");
                }
            }

            public void LoadFromFile()
            {
                try
                {
                    if (File.Exists("cart.json"))
                    {
                        var json = File.ReadAllText("cart.json");
                        Items = JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки корзины: {ex.Message}");
                }
                // реализована логика корзины: добавление, удаление товаров, изменение количества и подсчет общей суммы
            }
        }
        // модуль оформление заказа
        public class OrderService
        {
            private const string OrdersFilePath = "orders.json";

            public void SaveOrder(Order order)
            {
                try
                {
                    List<Order> orders = new List<Order>();
                    if (File.Exists(OrdersFilePath))
                    {
                        var json = File.ReadAllText(OrdersFilePath);
                        orders = JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();
                    }

                    // Генерируем уникальный номер заказа
                    order.OrderNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                    order.OrderDate = DateTime.Now;
                    orders.Add(order);

                    var updatedJson = JsonConvert.SerializeObject(orders);
                    File.WriteAllText(OrdersFilePath, updatedJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения заказа: {ex.Message}");
                }
            }

            public string GenerateOrderNumber()
            {
                return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }
            // добавлено оформление заказа, сбор данных, валидация и сохранение в JSON с уникальным номером
        }
        // Административная панель
        public class AdminService
        {
            private const string OrdersFilePath = "orders.json";
            private const string ProductsFilePath = "products.json";

            public List<Order> GetAllOrders()
            {
                try
                {
                    if (File.Exists(OrdersFilePath))
                    {
                        var json = File.ReadAllText(OrdersFilePath);
                        return JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка получения заказов: {ex.Message}");
                }
                return new List<Order>();
            }

            public void ChangeOrderStatus(string orderNumber, string newStatus)
            {
                try
                {
                    var orders = GetAllOrders();
                    var order = orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                    if (order != null)
                    {
                        order.Status = newStatus;
                        // сохранить
                        var json = JsonConvert.SerializeObject(orders);
                        File.WriteAllText(OrdersFilePath, json);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обновления статуса: {ex.Message}");
                }
            }
            //внедрена обработка ошибок try-catch, логирование и интеграция модулей для корректной работы системы
            // Методы для управления товарами аналогично
            //реализована административная панель для просмотра заказов и изменения их статусов
        }
        private Dictionary<string, List<string>> productsByCategory = new Dictionary<string, List<string>>()
        {
            { "Телефоны", new List<string> { "iPhone 14", "Samsung Galaxy S23", "Xiaomi Redmi" } },
            { "Ноутбуки", new List<string> { "Dell XPS 13", "MacBook Air", "Asus ROG" } },
            { "Планшеты", new List<string> { "iPad Pro", "Samsung Galaxy Tab", "Xiaomi Pad" } },
            { "Аксессуары", new List<string> { "Беспроводные наушники", "Клавиатура", "Мышь" } }
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.LightGray;

            // Заполняем ComboBox категориями
            comboBox1.Items.AddRange(new string[] { "Телефоны", "Ноутбуки", "Планшеты", "Аксессуары" });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBox1.SelectedItem.ToString();

            if (productsByCategory.ContainsKey(selectedCategory))
            {
                // Очищаем список товаров
                listBox1.Items.Clear();

                // Добавляем товары выбранной категории
                foreach (var product in productsByCategory[selectedCategory])
                {
                    listBox1.Items.Add(product);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();

            // Открываем форму как диалоговое окно (модальное)
            form2.ShowDialog();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }


        //Поиск определанного товара
        private List<string> allProducts = new List<string>()
{
            "iPhone 14", "Samsung Galaxy S23", "Xiaomi Redmi",
            "Dell XPS 13", "MacBook Air", "Asus ROG",
            "iPad Pro", "Samsung Galaxy Tab", "Xiaomi Pad",
            "Беспроводные наушники", "Клавиатура", "Мышь"
        };

       
        // Метод поиска и отображения результатов
        private void PerformSearch()
        {
            string searchText = textBox1.Text.ToLower(); // приводим к нижнему регистру для удобства
            listBox2.Items.Clear();

            foreach (var product in allProducts)
            {
                if (product.ToLower().Contains(searchText))
                {
                    listBox2.Items.Add(product);
                }
            }

            if (listBox2.Items.Count == 0)
            {
                listBox2.Items.Add("Нет результатов");
            }
        }
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = listBox2.SelectedItem?.ToString();
            if (selectedProduct != null)
            {
                MessageBox.Show($"Вы выбрали: {selectedProduct}", "Товар");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var products = new List<Product>
    {
        new Product { Name = "iPhone 14", Category = "Телефоны", Price = 799 },
        new Product { Name = "Samsung Galaxy S23", Category = "Телефоны", Price = 749 },
        new Product { Name = "Dell XPS 13", Category = "Ноутбуки", Price = 999 },
        new Product { Name = "Клавиатура", Category = "Аксессуары", Price = 50 },
    };

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "products.json");

            string jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText(filePath, jsonString);

            MessageBox.Show($"Файл сохранен по пути: {filePath}");
        
    }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            string email = textBoxEmail.Text.Trim();

            // Проверка, что поля не пустые
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Дополнительные проверки, например, формат email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return;
            }

            // Проверка уникальности логина
            if (IsLoginExist(login))
            {
                MessageBox.Show("Этот логин уже зарегистрирован. Выберите другой.");
                return;
            }

            // Регистрация (добавление пользователя в базу или список)
            RegisterUser(login, password, email);

            MessageBox.Show("Регистрация успешно завершена!");
        }

        // Метод проверки корректности email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Метод проверки существования логина
        private bool IsLoginExist(string login)
        {
            // Например, проверка в списке зарегистрированных пользователей
            // В реальности — база данных или файл
            return users.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        // Метод регистрации пользователя
        private void RegisterUser(string login, string password, string email)
        {
            // Добавьте нового пользователя в список или сохрани на диск
            users.Add(new User { Login = login, Password = password, Email = email });
        }

        // Класс для пользователя
        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        // Коллекция зарегистрированных пользователей
        private List<User> users = new List<User>();
    }

}
