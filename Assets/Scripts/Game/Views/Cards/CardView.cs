using Game.Models.Cards;
using Game.Views.Data;
using TMPro;
using UnityEngine;
using Zenject;

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
        [SerializeField] private SpriteRenderer _cardNoRenderer;
        private Card _card;
        private CardViewData _cardViewData;

        [Inject]
        public void Initialize(CardViewData cardViewData)
        {
            _cardViewData = cardViewData;
        }

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
            _cardTypeRenderer.sprite = _cardViewData.CardTypeSprites[(int) _card.CardType - 1];
            _cardNoRenderer.sprite = _cardViewData.CardNoSprites[(int) _card.CardNo - 1];
            var color = (_card.CardType == CardType.Hearts || _card.CardType == CardType.Diamonds)
                ? Color.red
                : Color.black;
            _cardNoRenderer.color = color;
            _cardTypeRenderer.color = color;
        }

        public class Pool : MonoMemoryPool<CardView> { }
    }
}
