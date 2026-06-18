import unittest
import requests  # если есть API, или импортировать ваши модули через API или командную строку
import os
import json


class TestCatalogService(unittest.TestCase):
    def test_load_products(self):
        # Предположим, что у вас есть API или файл
        # Например, через API:
        response = requests.get("http://localhost:5000/api/products")  # Замените на реальный URL
        self.assertEqual(response.status_code, 200)
        products = response.json()
        self.assertIsInstance(products, list)
        self.assertGreater(len(products), 0, "Нет товаров, тест не пройден")


class TestOrderProcess(unittest.TestCase):
    def test_create_order(self):
        # Создаем заказ через API или интерфейс
        data = {
            "CustomerName": "Test User",
            "Address": "Test Address",
            "Phone": "123456789",
            "Email": "test@example.com",
            "Items": [{"ProductId": 1, "Quantity": 2}]
        }
        response = requests.post("http://localhost:5000/api/orders", json=data)
        self.assertEqual(response.status_code, 201)
        order_info = response.json()
        self.assertIn("OrderNumber", order_info)


class TestAdminOperations(unittest.TestCase):
    def test_get_orders(self):
        response = requests.get("http://localhost:5000/api/admin/orders")
        self.assertEqual(response.status_code, 200)
        orders = response.json()
        self.assertIsInstance(orders, list)
        # Можно проверить что есть заказы
        self.assertGreaterEqual(len(orders), 0)


if __name__ == "__main__":
    unittest.main()