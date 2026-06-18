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
    internal class Module2_Cart
    {
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
        
    }
}
