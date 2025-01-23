using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;
using BeestjeOpJeFeestje.Data.Rules.BasketRules;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

namespace Tests
{
    public class ProductsMayNotBeTogetherTest
    {
        private ProductsMayNotBeTogether _rule;
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            _rule = new ProductsMayNotBeTogether();
            _basket = new Basket();
        }

        [Test]
        public void CheckProductsTogether_DangerousAnimalWithFarmAnimal_ReturnsFalse()
        {
            // Arrange
            var dangerousAnimal = new ProductDto { Name = "leeuw" };
            _basket.Products.Add(new ProductDto { Name = "koe", Type = Type.FARM });

            // Act
            var result = _rule.CheckProductsTogether(_basket, dangerousAnimal);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Nom Nom Nom", result.Item2);
        }

        [Test]
        public void CheckProductsTogether_FarmAnimalWithDangerousAnimal_ReturnsFalse()
        {
            // Arrange
            var farmAnimal = new ProductDto { Name = "koe", Type = Type.FARM };
            _basket.Products.Add(new ProductDto { Name = "leeuw" });

            // Act
            var result = _rule.CheckProductsTogether(_basket, farmAnimal);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Nom Nom Nom", result.Item2);
        }

        [Test]
        public void CheckProductsTogether_NoConflict_ReturnsTrue()
        {
            // Arrange
            var farmAnimal = new ProductDto { Name = "koe", Type = Type.FARM };
            _basket.Products.Add(new ProductDto { Name = "schaap", Type = Type.FARM });

            // Act
            var result = _rule.CheckProductsTogether(_basket, farmAnimal);

            // Assert
            Assert.IsTrue(result.Item1);
            Assert.IsEmpty(result.Item2);
        }
    }
}