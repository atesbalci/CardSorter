using System;
using Game.Models.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.Cards
{
    public class CardView : MonoBehaviour
    {
        private const float LerpMultiplier = 5f;
        private const float SelectedOffset = 0.5f;

        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }
        public bool Selected { get; set; }

        [SerializeField] private SpriteRenderer _cardTypeRenderer;
        [SerializeField] private TextMeshPro _cardNoText;
        [SerializeField] private Sprite[] _cardTypeSprites;
        private Card _card;

        public void Bind(Card card)
        {
            _card = card;
            _cardTypeRenderer.sprite = _cardTypeSprites[(int)_card.CardType];
            var color = (_card.CardType == CardType.Hearts || _card.CardType == CardType.Diamonds)
                ? Color.red
                : Color.black;
            _cardNoText.color = color;
            _cardTypeRenderer.color = color;
            var cardNoText = ((int) _card.CardNo).ToString();
            switch (_card.CardNo)
            {
                case CardNo.Ace:
                    cardNoText = "A";
                    break;
                case CardNo.Jack:
                    cardNoText = "J";
                    break;
                case CardNo.Queen:
                    cardNoText = "Q";
                    break;
                case CardNo.King:
                    cardNoText = "K";
                    break;
            }
            _cardNoText.text = cardNoText;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position,
                TargetPosition + (Selected ? SelectedOffset * Vector3.up : Vector3.zero),
                Time.deltaTime * LerpMultiplier);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.deltaTime * LerpMultiplier);
        }
    }
}
