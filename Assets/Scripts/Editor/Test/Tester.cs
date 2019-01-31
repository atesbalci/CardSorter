using System.Collections.Generic;
using Game.Models;
using Game.Models.Cards;
using NUnit.Framework;

namespace Editor.Test
{
    public class Tester
    {
        private IList<Card> GenerateTestCase()
        {
            return new List<Card>
            {
                new Card(CardNo.Ace, CardType.Spades),
                new Card(CardNo.Two, CardType.Spades),
                new Card(CardNo.Three, CardType.Spades),
                new Card(CardNo.Four, CardType.Spades),
                new Card(CardNo.Three, CardType.Diamonds),
                new Card(CardNo.Four, CardType.Diamonds),
                new Card(CardNo.Five, CardType.Diamonds),
                new Card(CardNo.Ace, CardType.Diamonds),
                new Card(CardNo.Ace, CardType.Hearts),
                new Card(CardNo.Four, CardType.Hearts),
                new Card(CardNo.Four, CardType.Clubs)
            };
        }

        #region 7-7-7

        [Test]
        public void SevenSevenSevenSingleCardLargestGroup()
        {
            var batch = GenerateTestCase();
            var result = GroupingAlgorithms.GetLargestSevenSevenSevenGroup(batch, new Card(CardNo.Four, CardType.Clubs));
            var expected = new List<Card>
            {
                new Card(CardNo.Four, CardType.Hearts),
                new Card(CardNo.Four, CardType.Diamonds),
                new Card(CardNo.Four, CardType.Spades),
                new Card(CardNo.Four, CardType.Clubs)
            };
            Assert.True(result.IsEquivalent(expected));
        }
        
        [Test]
        public void SevenSevenSevenSingleCardAllGroups()
        {
            var batch = GenerateTestCase();
            var result = GroupingAlgorithms.GetAllSevenSevenSevenGroups(batch, new Card(CardNo.Four, CardType.Clubs));
            var expected = new List<IList<Card>>
            {
                new List<Card>
                {
                    new Card(CardNo.Four, CardType.Hearts),
                    new Card(CardNo.Four, CardType.Diamonds),
                    new Card(CardNo.Four, CardType.Spades),
                    new Card(CardNo.Four, CardType.Clubs)
                },
                new List<Card>
                {
                    new Card(CardNo.Four, CardType.Hearts),
                    new Card(CardNo.Four, CardType.Diamonds),
                    new Card(CardNo.Four, CardType.Spades)
                },
                new List<Card>
                {
                    new Card(CardNo.Four, CardType.Hearts),
                    new Card(CardNo.Four, CardType.Diamonds),
                    new Card(CardNo.Four, CardType.Clubs)
                },
                new List<Card>
                {
                    new Card(CardNo.Four, CardType.Hearts),
                    new Card(CardNo.Four, CardType.Spades),
                    new Card(CardNo.Four, CardType.Clubs)
                },
                new List<Card>
                {
                    new Card(CardNo.Four, CardType.Diamonds),
                    new Card(CardNo.Four, CardType.Spades),
                    new Card(CardNo.Four, CardType.Clubs)
                }
            };
            Assert.True(result.IsEquivalent(expected, false));
        }

        #endregion

        #region 1-2-3

        [Test]
        public void OneTwoThreeSingleCardLargestGroup()
        {
            var batch = GenerateTestCase();
            var result = GroupingAlgorithms.GetLargestOneTwoThreeGroup(batch, new Card(CardNo.Four, CardType.Spades));
            var expected = new List<Card>
            {
                new Card(CardNo.Ace, CardType.Spades),
                new Card(CardNo.Two, CardType.Spades),
                new Card(CardNo.Three, CardType.Spades),
                new Card(CardNo.Four, CardType.Spades)
            };
            Assert.True(result.IsEquivalentInOrder(expected));
        }
        
        [Test]
        public void OneTwoThreeSingleCardAllGroups()
        {
            var batch = GenerateTestCase();
            var result = GroupingAlgorithms.GetAllOneTwoThreeGroups(batch, new Card(CardNo.Four, CardType.Spades));
            var expected = new List<IList<Card>>
            {
                new List<Card>
                {
                    new Card(CardNo.Ace, CardType.Spades),
                    new Card(CardNo.Two, CardType.Spades),
                    new Card(CardNo.Three, CardType.Spades),
                    new Card(CardNo.Four, CardType.Spades)
                },
                new List<Card>
                {
                    new Card(CardNo.Two, CardType.Spades),
                    new Card(CardNo.Three, CardType.Spades),
                    new Card(CardNo.Four, CardType.Spades)
                },
                new List<Card>
                {
                    new Card(CardNo.Ace, CardType.Spades),
                    new Card(CardNo.Two, CardType.Spades),
                    new Card(CardNo.Three, CardType.Spades)
                }
            };
            Assert.True(result.IsEquivalent(expected, true));
        }

        #endregion

        #region Smart Group
        
        [Test]
        public void SmartGroups()
        {
            var batch = GenerateTestCase();
            var result = GroupingAlgorithms.GetSmartGroups(batch);
            var expected = new CardGrouping
            {
                Groups =
                {
                    new List<Card>
                    {
                        new Card(CardNo.Ace, CardType.Spades),
                        new Card(CardNo.Two, CardType.Spades),
                        new Card(CardNo.Three, CardType.Spades)
                    },
                    new List<Card>
                    {
                        new Card(CardNo.Four, CardType.Spades),
                        new Card(CardNo.Four, CardType.Hearts),
                        new Card(CardNo.Four, CardType.Clubs)
                    },
                    new List<Card>
                    {
                        new Card(CardNo.Three, CardType.Diamonds),
                        new Card(CardNo.Four, CardType.Diamonds),
                        new Card(CardNo.Five, CardType.Diamonds)
                    }
                },
                Ungrouped =
                {
                    new Card(CardNo.Ace, CardType.Diamonds),
                    new Card(CardNo.Ace, CardType.Hearts)
                }
            };
            Assert.True(result.Groups.IsEquivalent(expected.Groups, false));
            Assert.True(result.Ungrouped.IsEquivalent(expected.Ungrouped));
        }

        #endregion
    }
}