﻿using System.Collections.Generic;
using Game.Models.Cards;
using Helpers.Utilities;

namespace Game.Models
{
    /// <summary>
    /// A static class containing static functions which will be used for card grouping an solving.
    /// Does contain some amount of list reallocations but the extra memory consumption because 
    /// of those would be negligible considering the lists they will be grouping will be relatively small.
    /// </summary>
    public static class GroupingAlgorithms
    {
        #region 7-7-7 Grouping

        /// <summary>
        /// Finds the largest 7-7-7 group containing the given card.
        /// </summary>
        public static IList<Card> GetLargestSevenSevenSevenGroup(IList<Card> cards, Card card)
        {
            var retVal = new List<Card>(4);
            foreach (var c in cards)
            {
                if (c.CardNo == card.CardNo)
                {
                    retVal.Add(c);
                }
            }
            return retVal.Count >= 3 ? retVal : new List<Card>(0);
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
                        var newList = new List<Card>(3);
                        for (int n = 0; n < largestGroup.Count; n++)
                        {
                            if (n != i)
                            {
                                newList.Add(largestGroup[n]);
                            }
                        }
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
            var candidates = new List<Card>();
            foreach (var c in cards)
            {
                if (c.CardType == card.CardType)
                {
                    int i;
                    for (i = 0; i < candidates.Count; i++)
                    {
                        if (candidates[i].CardNo > c.CardNo)
                        {
                            break;
                        }
                    }
                    candidates.Insert(i, c);
                }
            }
            int min, max, index;
            index = 0;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i].Equals(card))
                {
                    index = i;
                    break;
                }
            }
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
            return retVal.Count >= 3 ? retVal : new List<Card>();
        }

        /// <summary>
        /// Gets all permutations of 1-2-3 groups containing the card.
        /// </summary>
        public static IList<IList<Card>> GetAllOneTwoThreeGroups(IList<Card> cards, Card card)
        {
            return GetOneTwoThreePermutations(GetLargestOneTwoThreeGroup(cards, card).CloneList());
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
        public static CardGrouping GetSmartGroups(IList<Card> cards)
        {
            var allPossibleGroups = new List<IList<Card>>();

            var cardsOneTwoThreeClone = cards.CloneList();
            while (cardsOneTwoThreeClone.Count > 0)
            {
                var card = cardsOneTwoThreeClone[0];
                var oneTwoThreeGroups = GetAllOneTwoThreeGroups(cards, card);
                if (oneTwoThreeGroups.Count > 0)
                {
                    var largestGroup = oneTwoThreeGroups[0];
                    cardsOneTwoThreeClone.RemoveAll(largestGroup.Contains);
                    allPossibleGroups.AddRange(oneTwoThreeGroups);
                }
                else
                {
                    cardsOneTwoThreeClone.RemoveAt(0);
                }
            }
            
            var cardsSevenSevenSevenClone = cards.CloneList();
            while (cardsSevenSevenSevenClone.Count > 0)
            {
                var card = cardsSevenSevenSevenClone[0];
                var sevenSevenSevenGroups = GetAllSevenSevenSevenGroups(cards, card);
                if (sevenSevenSevenGroups.Count > 0)
                {
                    var largestGroup = sevenSevenSevenGroups[0];
                    cardsSevenSevenSevenClone.RemoveAll(largestGroup.Contains);
                    allPossibleGroups.AddRange(sevenSevenSevenGroups);
                }
                else
                {
                    cardsSevenSevenSevenClone.RemoveAt(0);
                }
            }

            var currentGrouping = new GroupingCandidate();
            var bestGrouping = new GroupingCandidate();
            
            FindBestGrouping(allPossibleGroups, currentGrouping, bestGrouping);
            
            var retVal = new CardGrouping(cards);
            foreach (var group in bestGrouping.Groups)
            {
                retVal.Groups.Add(group);
                foreach (var card in group)
                {
                    retVal.Ungrouped.Remove(card);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Recursive function used by GetSmartGroups.
        /// </summary>
        private static void FindBestGrouping(IList<IList<Card>> allPossibleGroups, GroupingCandidate currentGrouping, GroupingCandidate bestGrouping)
        {
            foreach (var possibleGroup in allPossibleGroups)
            {
                var compatible = true;
                for (var i = 0; i < currentGrouping.Groups.Count; i++)
                {
                    if (!currentGrouping.Groups[i].IsCompatible(possibleGroup))
                    {
                        compatible = false;
                        break;
                    }
                }

                if (compatible)
                {
                    currentGrouping.Push(possibleGroup);
                    FindBestGrouping(allPossibleGroups, currentGrouping, bestGrouping);

                    if (currentGrouping.Value > bestGrouping.Value)
                    {
                        bestGrouping.Clear();
                        foreach (var group in currentGrouping.Groups)
                        {
                            bestGrouping.Push(group);
                        }
                    }
                    
                    currentGrouping.Pop();
                }
            }
        }

        #endregion

        #region Helper Extension Methods

        /// <summary>
        /// Checks if two card collections are equivalent independently from how they are sorted.
        /// </summary>
        public static bool IsEquivalent(this ICollection<Card> a, ICollection<Card> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }
            
            foreach (var card1 in a)
            {
                var contains = false;
                foreach (var card2 in b)
                {
                    if (card1.Equals(card2))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if two card group lists are equivalent independently from how they are sorted.
        /// </summary>
        public static bool IsEquivalent(this ICollection<IList<Card>> a, ICollection<IList<Card>> b, bool inGroupOrdered)
        {
            if (a.Count != b.Count)
            {
                return false;
            }
            
            foreach (var group1 in a)
            {
                var contains = false;
                foreach (var group2 in b)
                {
                    if (inGroupOrdered ? group1.IsEquivalentInOrder(group2) : group1.IsEquivalent(group2))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if two card collections are equivalent.
        /// </summary>
        public static bool IsEquivalentInOrder(this IList<Card> a, IList<Card> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0; i < a.Count; i++)
            {
                if (!a[i].Equals(b[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two card lists contain all different cards.
        /// </summary>
        private static bool IsCompatible(this IList<Card> a, IList<Card> b)
        {
            for (var i = 0; i < a.Count; i++)
            {
                if (b.Contains(a[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates the sum of values of the cards in a list.
        /// </summary>
        private static int GetValue(this IList<Card> cards)
        {
            var sum = 0;
            for (var i = 0; i < cards.Count; i++)
            {
                sum += cards[i].Value;
            }

            return sum;
        }

        #endregion

        #region Nested Classes

        private class GroupingCandidate
        {
            public int Value { get; private set; }
            public IList<IList<Card>> Groups { get; }

            public GroupingCandidate()
            {
                Groups = new List<IList<Card>>();
                Value = 0;
            }

            public void Push(IList<Card> group)
            {
                Groups.Add(group);
                Value += group.GetValue();
            }

            public void Pop()
            {
                var lastIndex = Groups.Count - 1;
                Value -= Groups[lastIndex].GetValue();
                Groups.RemoveAt(lastIndex);
            }

            public void Clear()
            {
                Groups.Clear();
                Value = 0;
            }
        }

        #endregion
    }

    #region Helper Class

    public class CardGrouping
    {
        public ICollection<IList<Card>> Groups { get; }
        public IList<Card> Ungrouped { get; }

        public CardGrouping()
        {
            Groups = new LinkedList<IList<Card>>();
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

        public int UngroupedValue
        {
            get
            {
                var sum = 0;
                foreach (var card in Ungrouped)
                {
                    sum += card.Value;
                }
                return sum;
            }
        }

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
