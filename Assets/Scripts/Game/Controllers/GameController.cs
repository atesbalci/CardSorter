﻿using System;
using Game.Models.Cards;
using Game.Models.Data;
using Game.Views.Cards;
using Helpers.Utilities;
using UnityEngine;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        public HandView HandView;

        private CardBatch _hand;
        private CardBatch _deck;

        private void Start()
        {
            Initialize();
            StartGame();
        }

        private void Initialize()
        {
            _hand = new CardBatch();
            _deck = new CardBatch();
            var typeAmt = Enum.GetValues(typeof(CardType)).Length;
            var noAmt = Enum.GetValues(typeof(CardNo)).Length;
            for (CardType t = CardType.None + 1; (int) t < typeAmt; t++)
            {
                for (CardNo no = CardNo.None + 1; (int) no < noAmt; no++)
                {
                    _deck.Add(new Card(no, t));
                }
            }
            HandView.Bind(_hand);
        }

        public void StartGame()
        {
            var handAmt = _hand.Count;
            for (int i = 0; i < handAmt; i++)
            {
                _deck.Add(_hand[0]);
                _hand.RemoveAt(0);
            }

            for (int i = 0; i < GameRules.CardsToDraw; i++)
            {
                _hand.Add(_deck.RandomElementAndRemove());
            }
        }
    }
}
