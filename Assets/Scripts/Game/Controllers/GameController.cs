using System;
using System.Collections;
using Game.Models;
using Game.Models.Cards;
using Game.Views.Cards;
using Helpers.Utilities;
using UniRx;
using UnityEngine;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private const float DrawFrequency = 0.1f;

        public HandView HandView;

        private CardBatch _hand;
        private CardBatch _fullDeck;
        private CardBatch _testCaseDeck;
        private CardBatch _lastUsedDeck;
        private IDisposable _drawDisposable;

        private void Start()
        {
            Initialize();
            StartGame();
        }

        private void Initialize()
        {
            _hand = new CardBatch();
            _fullDeck = new CardBatch();
            var typeAmt = Enum.GetValues(typeof(CardType)).Length;
            var noAmt = Enum.GetValues(typeof(CardNo)).Length;
            for (CardType t = CardType.None + 1; (int) t < typeAmt; t++)
            {
                for (CardNo no = CardNo.None + 1; (int) no < noAmt; no++)
                {
                    _fullDeck.Add(new Card(no, t));
                }
            }
            _testCaseDeck = new CardBatch
            {
                new Card(CardNo.Ace, CardType.Diamonds),
                new Card(CardNo.Four, CardType.Clubs),
                new Card(CardNo.Three, CardType.Diamonds),
                new Card(CardNo.Four, CardType.Diamonds),
                new Card(CardNo.Five, CardType.Diamonds),
                new Card(CardNo.Three, CardType.Spades),
                new Card(CardNo.Four, CardType.Spades),
                new Card(CardNo.Ace, CardType.Spades),
                new Card(CardNo.Ace, CardType.Hearts),
                new Card(CardNo.Four, CardType.Hearts),
                new Card(CardNo.Two, CardType.Spades),
            };
            HandView.Bind(_hand);
            _lastUsedDeck = _fullDeck;
        }

        /// <summary>
        /// Draws random cards.
        /// </summary>
        public void StartGame()
        {
            ClearHand();
            Draw(_fullDeck);
        }

        /// <summary>
        /// Draws test case. Added since random groups often don't group well.
        /// </summary>
        public void StartTestCase()
        {
            ClearHand();
            Draw(_testCaseDeck);
        }

        private void ClearHand()
        {
            var handAmt = _hand.Count;
            for (int i = 0; i < handAmt; i++)
            {
                _lastUsedDeck.Add(_hand[0]);
                _hand.RemoveAt(0);
            }
        }

        /// <summary>
        /// Spawns the cards one by one.
        /// </summary>
        private void Draw(CardBatch deck)
        {
            _drawDisposable?.Dispose();
            _lastUsedDeck = deck;
            var cardsToDraw = Mathf.Min(GameRules.CardsToDraw, deck.Count);
            var i = 0;
            _drawDisposable = Observable.Interval(TimeSpan.FromSeconds(DrawFrequency)).Subscribe(l =>
            {
                i++;
                _hand.Add(deck.RandomElementAndRemove());
                if (i >= cardsToDraw)
                {
                    _drawDisposable.Dispose();
                }
            });
        }
    }
}
