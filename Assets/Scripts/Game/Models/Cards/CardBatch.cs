using System;
using System.Collections;
using System.Collections.Generic;
using Game.Models.Data;

namespace Game.Models.Cards
{
    public class CardBatch : IList<Card>
    {
        /// <summary>
        /// Represents a grouping function for a single card within a batch.
        /// </summary>
        private delegate IList<Card> GroupingMethod(IList<Card> cards, Card card);

        public event Action OnChange;

        private IList<Card> _cards;

        public CardBatch()
        {
            _cards = new List<Card>();
        }

        #region Sorting Methods

        /// <summary>
        /// Used when a single group sorting is sufficient. 
        /// </summary>
        public void SortByGrouping(GroupType groupType)
        {
            GroupingMethod groupFunc;
            switch (groupType)
            {
                case GroupType.OneTwoThree:
                    groupFunc = GroupingAlgorithms.GetLargestOneTwoThreeGroup;
                    break;
                case GroupType.SevenSevenSeven:
                    groupFunc = GroupingAlgorithms.GetLargestSevenSevenSevenGroup;
                    break;
                default:
                    return;
            }
            var cardGrouping = new CardGrouping();
            while (_cards.Count > 0)
            {
                var card = _cards[0];
                var group = groupFunc(_cards, card);
                if (group.Count > 0)
                {
                    cardGrouping.Groups.Add(group);
                    foreach (var c in group)
                    {
                        _cards.Remove(c);
                    }
                }
                else
                {
                    _cards.RemoveAt(0);
                    cardGrouping.Ungrouped.Add(card);
                }
            }
            _cards = cardGrouping.ToFlatList();
            OnChange?.Invoke();
        }

        /// <summary>
        /// Sorts the batch by smart grouping.
        /// </summary>
        public void SortBySmartGrouping()
        {
            _cards = GroupingAlgorithms.GetSmartGroups(_cards).ToFlatList();
            OnChange?.Invoke();
        }

        #endregion

        #region IList Member Methods

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Card item)
        {
            _cards.Add(item);
            OnChange?.Invoke();
        }

        public void Clear()
        {
            _cards.Clear();
            OnChange?.Invoke();
        }

        public bool Contains(Card item)
        {
            return _cards.Contains(item);
        }

        public void CopyTo(Card[] array, int arrayIndex)
        {
            _cards.CopyTo(array, arrayIndex);
        }

        public bool Remove(Card item)
        {
            var retVal = _cards.Remove(item);
            OnChange?.Invoke();
            return retVal;
        }

        public int Count => _cards.Count;
        public bool IsReadOnly => _cards.IsReadOnly;

        public int IndexOf(Card item)
        {
            return _cards.IndexOf(item);
        }

        public void Insert(int index, Card item)
        {
            _cards.Insert(index, item);
            OnChange?.Invoke();
        }

        public void RemoveAt(int index)
        {
            _cards.RemoveAt(index);
            OnChange?.Invoke();
        }

        public Card this[int index]
        {
            get => _cards[index];
            set
            {
                _cards[index] = value;
                OnChange?.Invoke();
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a single grouping algorithm type.
    /// </summary>
    public enum GroupType
    {
        None,
        OneTwoThree,
        SevenSevenSeven
    }
}
