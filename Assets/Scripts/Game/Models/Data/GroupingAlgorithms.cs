using System.Collections.Generic;
using System.Linq;
using Game.Models.Cards;

namespace Game.Models.Data
{
    public static class GroupingAlgorithms
    {
        #region 7-7-7 Grouping

        /// <summary>
        /// Finds the largest 7-7-7 group containing the given card.
        /// </summary>
        public static IList<Card> GetLargestSevenSevenSevenGroup(IList<Card> cards, Card card)
        {
            var retVal = cards.Where(c => c.CardNo == card.CardNo).ToList();
            return retVal;
        }

        /// <summary>
        /// Finds all the permutations of card 7-7-7 groups containing 3 or 4 cards.
        /// </summary>
        public static IList<IList<Card>> GetAllSevenSevenSevenGroups(IList<Card> cards, Card card)
        {
            var retVal = new List<IList<Card>>();
            var largestGroup = GetLargestSevenSevenSevenGroup(cards, card);
            if (largestGroup.Count >= 3)
            {
                retVal.Add(largestGroup);
                if (largestGroup.Count > 3)
                {
                    for (var i = 0; i < largestGroup.Count; i++)
                    {
                        var newList = largestGroup.ToList();
                        newList.RemoveAt(i);
                        retVal.Add(newList);
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region 1-2-3 Grouping

        /// <summary>
        /// Finds the largest 1-2-3 group containing the given card.
        /// </summary>
        public static IList<Card> GetLargestOneTwoThreeGroup(IList<Card> cards, Card card)
        {
            var candidates = cards.Where(c => c.CardType == card.CardType).OrderBy(c => c.CardNo).ToList();
            int min, max, index;
            index = candidates.IndexOf(card);
            min = max = index;
            for (int i = index + 1; i < candidates.Count; i++)
            {
                if (candidates[i].CardNo == candidates[max].CardNo + 1)
                {
                    max = i;
                }
                else
                {
                    break;
                }
            }
            for (int i = index - 1; i >= 0; i--)
            {
                if (candidates[i].CardNo == candidates[min].CardNo - 1)
                {
                    min = i;
                }
                else
                {
                    break;
                }
            }
            var retVal = candidates.GetRange(min, max - min + 1);
            return retVal;
        }

        /// <summary>
        /// Gets all permutations of 1-2-3 groups containing the card.
        /// </summary>
        public static IList<IList<Card>> GetAllOneTwoThreeGroups(IList<Card> cards, Card card)
        {
            return GetOneTwoThreePermutations(GetLargestOneTwoThreeGroup(cards, card).ToList());
        }

        /// <summary>
        /// Recursive function used by GetAllOneTwoThreeGroups.
        /// </summary>
        private static IList<IList<Card>> GetOneTwoThreePermutations(List<Card> cards)
        {
            var retVal = new List<IList<Card>>();
            if (cards.Count >= 3)
            {
                retVal.Add(cards);
                retVal.AddRange(GetOneTwoThreePermutations(cards.GetRange(0, cards.Count - 1)));
                retVal.AddRange(GetOneTwoThreePermutations(cards.GetRange(1, cards.Count - 1)));
            }
            return retVal;
        }

        #endregion

        #region Smart Grouping

        /// <summary>
        /// Finds the smart group by testing and mixing both 1-2-3 sorting and 7-7-7 sorting. 
        /// Returns the grouping with the least total value of ungrouped cards.
        /// </summary>
        public static CardGrouping GetSmartGroups(IEnumerable<Card> cards)
        {
            return GetSmartGroups(new CardGrouping(cards));
        }

        /// <summary>
        /// Recursive function used by GetSmartGroups.
        /// </summary>
        private static CardGrouping GetSmartGroups(CardGrouping cardGrouping)
        {
            var bestGrouping = cardGrouping;
            var cards = cardGrouping.Ungrouped.ToList();
            while (cards.Count > 0)
            {
                var curCard = cards[0];
                var possibilities = GetAllOneTwoThreeGroups(cards, curCard)
                    .Union(GetAllSevenSevenSevenGroups(cards, curCard));
                foreach (var group in possibilities)
                {
                    var grouping = cardGrouping.Clone();
                    grouping.Groups.Add(group);
                    foreach (var card in group)
                    {
                        grouping.Ungrouped.Remove(card);
                    }
                    grouping = GetSmartGroups(grouping);
                    if (bestGrouping == null || bestGrouping.UngroupedValue > grouping.UngroupedValue)
                    {
                        bestGrouping = grouping;
                    }
                }
                cards.RemoveAt(0);
            }
            return bestGrouping;
        }

        #endregion
    }

    #region Helper Class

    public class CardGrouping
    {
        public IList<IList<Card>> Groups { get; }
        public IList<Card> Ungrouped { get; }

        public CardGrouping()
        {
            Groups = new List<IList<Card>>();
            Ungrouped = new List<Card>();
        }

        public CardGrouping(IEnumerable<Card> cards) : this()
        {
            foreach (var card in cards)
            {
                Ungrouped.Add(card);
            }
        }

        public IList<Card> ToFlatList()
        {
            var retVal = new List<Card>();
            foreach (var group in Groups)
            {
                retVal.AddRange(group);
            }
            retVal.AddRange(Ungrouped);
            return retVal;
        }

        public int UngroupedValue => Ungrouped.Sum(card => card.Value);

        public CardGrouping Clone()
        {
            var retVal = new CardGrouping();
            foreach (var card in Ungrouped)
            {
                retVal.Ungrouped.Add(card);
            }
            foreach (var card in Groups)
            {
                retVal.Groups.Add(card);
            }
            return retVal;
        }
    }

    #endregion
}
