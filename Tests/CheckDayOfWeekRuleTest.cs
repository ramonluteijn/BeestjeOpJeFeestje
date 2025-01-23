using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using NUnit.Framework;
using System;

namespace Tests
{
    public class CheckDayOfWeekRuleTest
    {
        private CheckDayOfWeekRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new CheckDayOfWeekRule();
        }

        [Test]
        public void IsDayOfWeek_Monday_Returns15()
        {
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 2) }; // Monday
            var result = _rule.IsDayOfWeek(orderDto);
            Assert.AreEqual(15, result);
        }

        [Test]
        public void IsDayOfWeek_Tuesday_Returns15()
        {
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 3) }; // Tuesday
            var result = _rule.IsDayOfWeek(orderDto);
            Assert.AreEqual(15, result);
        }

        [Test]
        public void IsDayOfWeek_Wednesday_Returns0()
        {
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 4) }; // Wednesday
            var result = _rule.IsDayOfWeek(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void IsDayOfWeek_Sunday_Returns0()
        {
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 1) }; // Sunday
            var result = _rule.IsDayOfWeek(orderDto);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void IsDayOfWeek_NullOrderDto_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _rule.IsDayOfWeek(null));
        }
    }
}