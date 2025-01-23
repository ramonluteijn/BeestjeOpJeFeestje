using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using NUnit.Framework;
using System.Collections.Generic;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace Tests
{
    public class SameTypeRuleTest
    {
        private SameTypeRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new SameTypeRule();
        }

        [Test]
        public void CheckSameType_ThreeOrMoreSameTypeProducts_Returns10()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM} },
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM} },
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM} }
                }
            };
            var result = _rule.CheckSameType(orderDto);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void CheckSameType_LessThanThreeSameTypeProducts_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM } }
                }
            };
            var result = _rule.CheckSameType(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckSameType_MixedTypeProducts_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.DESERT } },
                    new OrderDetailsDto { Product = new ProductDto { Type = Type.SNOW} }
                }
            };
            var result = _rule.CheckSameType(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckSameType_EmptyOrderDetails_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };
            var result = _rule.CheckSameType(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckSameType_NullOrderDto_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _rule.CheckSameType(null));
        }
    }
}