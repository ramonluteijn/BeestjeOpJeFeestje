using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class NameContainsRuleTest
    {
        private NameContainsRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new NameContainsRule();
        }

        [Test]
        public void ApplyNameContainsDiscount_SingleProductWithUniqueLetters_ReturnsCorrectDiscount()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "abc" } }
                }
            };
            var result = _rule.ApplyNameContainsDiscount(orderDto);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void ApplyNameContainsDiscount_MultipleProductsWithUniqueLetters_ReturnsCorrectDiscount()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "abc" } },
                    new OrderDetailsDto { Product = new ProductDto { Name = "def" } }
                }
            };
            var result = _rule.ApplyNameContainsDiscount(orderDto);
            Assert.AreEqual(12, result);
        }

        [Test]
        public void ApplyNameContainsDiscount_ProductWithDuplicateLetters_ReturnsCorrectDiscount()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "aabbcc" } }
                }
            };
            var result = _rule.ApplyNameContainsDiscount(orderDto);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void ApplyNameContainsDiscount_EmptyOrderDetails_ReturnsZero()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };
            var result = _rule.ApplyNameContainsDiscount(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ApplyNameContainsDiscount_NullOrderDto_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _rule.ApplyNameContainsDiscount(null));
        }
    }
}