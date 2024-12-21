using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;

namespace BeestjeOpJeFeestje.Data.Services
{
    public class BasketService
    {
        private readonly Basket basket;
        public BasketService()
        {
            basket = new Basket();
        }
        public void AddToBasket(ProductDto product)
        {
            basket.Products.Add(product);
            product.IsInBasket = true;
        }
        public void RemoveFromBasket(int productId)
        {
            var product = basket.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                basket.Products.Remove(product);
                product.IsInBasket = false;
            }
        }
        public List<ProductDto> GetBasketProducts()
        {
            return basket.Products;
        }
        public void ClearBasket()
        {
            basket.Products.Clear();
        }
        public int GetBasketItemCount()
        {
            return basket.Products.Count;
        }
    }
}