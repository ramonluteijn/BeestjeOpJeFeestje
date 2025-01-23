using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class PayForMoreProductsTest
    {
        private PayForMoreProducts _rule;
        private List<ProductDto> _allProducts;

        [SetUp]
        public void Setup()
        {
            _rule = new PayForMoreProducts();
            _allProducts = new List<ProductDto>
            {
                new ProductDto { Name = "Product1", Price = 10 },
                new ProductDto { Name = "Product2", Price = 20 },
                new ProductDto { Name = "Product3", Price = 30 }
            };
        }

        [Test]
        public void PayForExtraProducts_OrderDetailsCountIsMultipleOfRequiredAmount_ReturnsRandomProductPrice()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "Product1" } },
                    new OrderDetailsDto { Product = new ProductDto { Name = "Product2" } }
                }
            };
            var result = _rule.PayForExtraProducts(orderDto, _allProducts);
            Assert.Contains(result, new List<int> { 10, 20, 30 });
        }

        [Test]
        public void PayForExtraProducts_OrderDetailsCountIsNotMultipleOfRequiredAmount_ReturnsZero()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "Product1" } }
                }
            };
            var result = _rule.PayForExtraProducts(orderDto, _allProducts);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void PayForExtraProducts_EmptyOrderDetails_ReturnsZero()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };
            var result = _rule.PayForExtraProducts(orderDto, _allProducts);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void PayForExtraProducts_NullOrderDto_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _rule.PayForExtraProducts(null, _allProducts));
        }
    }
}