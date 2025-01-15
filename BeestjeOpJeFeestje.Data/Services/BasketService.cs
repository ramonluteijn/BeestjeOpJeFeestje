using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;
using BeestjeOpJeFeestje.Data.Rules.BasketRules;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Data.Services
{
    public class BasketService
    {
        private readonly Basket basket;
        private readonly UserManager<User> userManager;
        public BasketService()
        {
            basket = new Basket();
        }

        public (bool, string) AddToBasket(ProductDto product, int? userId = null)
        {
            var (checkBasket, result) = CheckBasket(userId, product);
            if (!checkBasket)
            {
                return (false, result);
            }

            basket.Products.Add(product);
            product.IsInBasket = true;
            return (true, "Product added to basket successfully.");
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

        private (bool, string) CheckBasket(int? userId = null, ProductDto product = null)
        {
            var user = userId != null ? userManager.FindByIdAsync(userId.Value.ToString()).Result : null;

            var (checkProducts, resultProducts) = new CheckOrderProductsRule().CheckProducts(basket, user, product);
            if (!checkProducts) return (false, resultProducts);

            var (checkTogether, resultTogether) = new ProductsMayNotBeTogether().CheckProductsTogether(basket, product);
            if (!checkTogether) return (false, resultTogether);

            var(check, result) = new CheckSeasonRule().CheckAnimalAvailability(basket, product);
            return !check ? (false, result) : (true, string.Empty);
        }
    }
}