using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;
using BeestjeOpJeFeestje.Data.Rules.BasketRules;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
using NUnit.Framework;
using System.Collections.Generic;
using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Moq;

namespace Tests
{
    public class CheckOrderProductRuleTest
    {
        private CheckOrderProductsRule _rule;
        private Basket _basket;
        private Mock<User> _userMock;

        [SetUp]
        public void Setup()
        {
            _rule = new CheckOrderProductsRule();
            _basket = new Basket();
            _userMock = new Mock<User>();
        }

        [Test]
        public void CheckProducts_NoUser_TooManyProducts_ReturnsFalse()
        {
            // Arrange
            _basket.Products.AddRange(new List<ProductDto>
            {
                new ProductDto { Type = Type.DESERT },
                new ProductDto { Type = Type.FARM },
                new ProductDto { Type = Type.SNOW }
            });

            // Act
            var result = _rule.CheckProducts(_basket, null);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("You have too many products in your order, only 3 products are allowed and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_NoUser_VipProduct_ReturnsFalse()
        {
            // Arrange
            var newProduct = new ProductDto { Type = Type.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, null, newProduct);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("You have too many products in your order, only 3 products are allowed and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_SilverUser_TooManyProducts_ReturnsFalse()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.SILVER);
            _basket.Products.AddRange(new List<ProductDto>
            {
                new ProductDto { Type = Type.DESERT },
                new ProductDto { Type = Type.FARM },
                new ProductDto { Type = Type.SNOW },
                new ProductDto { Type = Type.JUNGLE }
            });

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Silver members can only have up to 4 products and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_GoldUser_VipProduct_ReturnsFalse()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.GOLD);
            var newProduct = new ProductDto { Type = Type.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object, newProduct);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Only VIP users can add VIP products to the basket.", result.Item2);
        }

        [Test]
        public void CheckProducts_PlatinumUser_VipProduct_ReturnsTrue()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.PLATINUM);
            var newProduct = new ProductDto { Type = Type.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object, newProduct);

            // Assert
            Assert.IsTrue(result.Item1);
            Assert.IsEmpty(result.Item2);
        }
    }
}