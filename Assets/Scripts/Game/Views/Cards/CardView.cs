using Game.Models.Cards;
using TMPro;
using UnityEngine;

namespace Game.Views.Cards
{
    /// <summary>
    /// View class for card.
    /// </summary>
    public class CardView : MonoBehaviour
    {
        private const float LerpMultiplier = 5f;
        private const float SelectedLerpMultiplier = 15f;
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
            RefreshCardVisuals();
        }

        private void Update()
        {
            var lerpFactor = (Selected ? SelectedLerpMultiplier : LerpMultiplier) * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position,
                TargetPosition + (Selected ? SelectedOffset * Vector3.up : Vector3.zero),
                lerpFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, lerpFactor);
        }

        private void RefreshCardVisuals()
        {
            _cardTypeRenderer.sprite = _cardTypeSprites[(int)_card.CardType];
            var color = (_card.CardType == CardType.Hearts || _card.CardType == CardType.Diamonds)
                ? Color.red
                : Color.black;
            _cardNoText.color = color;
            _cardTypeRenderer.color = color;
            string cardNoText;
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
                default:
                    cardNoText = ((int) _card.CardNo).ToString();
                    break;
            }
            _cardNoText.text = cardNoText;
        }
    }
}
