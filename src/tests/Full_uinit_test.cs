using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InternetMagazin.Form1;

namespace InternetMagazin
{
    internal class unit_test1
    {
        private void RunFullTestsAndSaveResults()
        {
            StringBuilder results = new StringBuilder();

            results.AppendLine("=== Тест CatalogService ===");
            results.AppendLine(TestCatalogService());

            results.AppendLine("\n=== Тест OrderService ===");
            results.AppendLine(TestOrderService());

            results.AppendLine("\n=== Тест AdminService ===");
            results.AppendLine(TestAdminService());

            // Сохраняем в файл
            string filePath = "TestResults.txt";
            try
            {
                File.WriteAllText(filePath, results.ToString());
                Console.WriteLine($"Результаты тестов сохранены в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении результатов: {ex.Message}");
            }
        }

        // Обновим функции для возвращения строковых результатов
        private string TestCatalogService()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                CatalogService catalog = new CatalogService();
                var products = catalog.LoadProducts();

                sb.AppendLine($"Загружено товаров: {products.Count}");
                if (products.Count > 0)
                {
                    sb.AppendLine($"Первый товар: {products[0].Name}, цена: {products[0].Price}");
                    sb.AppendLine("Тест загрузки прошел успешно");
                }
                else
                {
                    sb.AppendLine("Тест загрузки не прошел: список пуст");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Исключение: {ex.Message}");
            }
            return sb.ToString();
        }

        private string TestOrderService()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                OrderService orderService = new OrderService();

                Order order = new Order
                {
                    CustomerName = "Тестовый клиент",
                    Address = "ул. Тестовая, дом 1",
                    Phone = "123456",
                    Email = "test@test.com",
                    Status = "Новый",
                    Items = new List<CartItem> { new CartItem { Product = new Product { Id = 1, Name = "Test", Price = 10 }, Quantity = 2 } }
                };

                orderService.SaveOrder(order);
                sb.AppendLine("Заказ успешно сохранен");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Ошибка: {ex.Message}");
            }
            return sb.ToString();
        }

        private string TestAdminService()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                AdminService admin = new AdminService();
                var orders = admin.GetAllOrders();
                sb.AppendLine($"Всего заказов: {orders.Count}");

                if (orders.Count > 0)
                {
                    string orderNumber = orders[0].OrderNumber;
                    admin.ChangeOrderStatus(orderNumber, "Обработан");
                    sb.AppendLine($"Статус заказа {orderNumber} обновлен");
                }
                else
                {
                    sb.AppendLine("Нет заказов для обновления статуса");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Исключение: {ex.Message}");
            }
            return sb.ToString();
        }
    }
}
