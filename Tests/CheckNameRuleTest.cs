using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class CheckNameRuleTest
    {
        private CheckNameRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new CheckNameRule();
        }

        [Test]
        public void CheckForName_OrderContainsEend_Returns50Or0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "eend" } }
                }
            };
            var result = _rule.CheckForName(orderDto);
            Assert.IsTrue(result == 50 || result == 0);
        }

        [Test]
        public void CheckForName_OrderDoesNotContainEend_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "kat" } }
                }
            };
            var result = _rule.CheckForName(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckForName_OrderContainsEendCaseInsensitive_Returns50Or0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "EEND" } }
                }
            };
            var result = _rule.CheckForName(orderDto);
            Assert.IsTrue(result == 50 || result == 0);
        }

        [Test]
        public void CheckForName_OrderDetailsIsEmpty_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };
            var result = _rule.CheckForName(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckForName_NullOrderDto_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _rule.CheckForName(null));
        }
    }
}