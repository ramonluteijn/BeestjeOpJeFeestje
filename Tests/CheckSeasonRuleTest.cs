using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;
using BeestjeOpJeFeestje.Data.Rules.BasketRules;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class CheckSeasonRuleTest
    {
        private CheckSeasonRule _rule;
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            _rule = new CheckSeasonRule();
            _basket = new Basket();
        }

        [Test]
        public void CheckAnimalAvailability_PenguinOnWeekend_ReturnsFalse()
        {
            // Arrange
            var product = new ProductDto { Name = "Pinguïn" };
            var currentDate = new DateTime(2023, 10, 7); // Saturday
            SystemTime.SetDateTime(currentDate);

            // Act
            var result = _rule.CheckAnimalAvailability(_basket, product);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Dieren in pak werken alleen doordeweek", result.Item2);
        }

        [Test]
        public void CheckAnimalAvailability_DesertAnimalInWinter_ReturnsFalse()
        {
            // Arrange
            var product = new ProductDto { Type = Type.DESERT };
            var currentDate = new DateTime(2023, 12, 1); // Winter
            SystemTime.SetDateTime(currentDate);

            // Act
            var result = _rule.CheckAnimalAvailability(_basket, product);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Veelste koud", result.Item2);
        }

        [Test]
        public void CheckAnimalAvailability_SnowAnimalInSummer_ReturnsFalse()
        {
            // Arrange
            var product = new ProductDto { Type = Type.SNOW };
            var currentDate = new DateTime(2023, 7, 1); // Summer
            SystemTime.SetDateTime(currentDate);

            // Act
            var result = _rule.CheckAnimalAvailability(_basket, product);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Some People Are Worth Melting For. ~ Ola", result.Item2);
        }

        [Test]
        public void CheckAnimalAvailability_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var product = new ProductDto { Name = "Kameel", Type = Type.DESERT };
            var currentDate = new DateTime(2023, 5, 1); // Spring
            SystemTime.SetDateTime(currentDate);

            // Act
            var result = _rule.CheckAnimalAvailability(_basket, product);

            // Assert
            Assert.IsTrue(result.Item1);
            Assert.IsEmpty(result.Item2);
        }
    }
}