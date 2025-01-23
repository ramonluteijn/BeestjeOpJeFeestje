using BeestjeOpJeFeestje.Data.Rules.ValidationRules;
using NUnit.Framework;

namespace Tests
{
    public class PostalCodeRuleTest
    {
        private PostalCodeRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new PostalCodeRule();
        }

        [Test]
        public void IsValid_ValidPostalCode_ReturnsTrue()
        {
            // Arrange
            var validPostalCode = "1234AB";

            // Act
            var result = _rule.IsValid(validPostalCode);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_InvalidPostalCode_ReturnsFalse()
        {
            // Arrange
            var invalidPostalCode = "123AB";

            // Act
            var result = _rule.IsValid(invalidPostalCode);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValid_PostalCodeWithSpaces_ReturnsTrue()
        {
            // Arrange
            var postalCodeWithSpaces = "1234 AB";

            // Act
            var result = _rule.IsValid(postalCodeWithSpaces);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_NullValue_ReturnsFalse()
        {
            // Act
            var result = _rule.IsValid(null);

            // Assert
            Assert.IsFalse(result);
        }
    }
}