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
    internal class Module1_Catalog
    {
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
    }
}
