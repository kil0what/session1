using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsGoodsApp
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.Price * Quantity;
    }

    public static class Cart
    {
        private static List<CartItem> items = new List<CartItem>();

        public static List<CartItem> Items => items;
        public static decimal TotalAmount => items.Sum(i => i.TotalPrice);
        public static int TotalItems => items.Sum(i => i.Quantity);

        public static void AddProduct(Product product, int quantity = 1)
        {
            var existing = items.FirstOrDefault(i => i.Product.ArticleNumber == product.ArticleNumber);
            if (existing != null)
                existing.Quantity += quantity;
            else
                items.Add(new CartItem { Product = product, Quantity = quantity });
        }

        public static void Clear()
        {
            items.Clear();
        }
    }
}