using BeestjeOpJeFeestje.Data.Rules;
using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using NUnit.Framework;

namespace Tests
{
    public class HasRankRuleTest
    {
        private HasRankRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new HasRankRule();
        }

        [Test]
        public void UserHasRank_UserWithRank_Returns10()
        {
            var user = new User { Rank = Rank.GOLD };
            var result = _rule.UserHasRank(user);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void UserHasRank_UserWithoutRank_Returns0()
        {
            var user = new User { Rank = Rank.NONE };
            var result = _rule.UserHasRank(user);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void UserHasRank_NullUser_Returns0()
        {
            var result = _rule.UserHasRank(null);
            Assert.AreEqual(0, result);
        }
    }
}