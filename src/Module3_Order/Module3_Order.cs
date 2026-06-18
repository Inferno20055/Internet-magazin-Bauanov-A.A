using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InternetMagazin.Form1;

namespace InternetMagazin
{
    internal class Module3_Order
    {
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
    }
}
