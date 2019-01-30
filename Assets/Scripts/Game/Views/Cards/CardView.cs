using DG.Tweening;
using Game.Models.Cards;
using Game.Views.Data;
using Helpers.Vectors;
using UnityEngine;
using Zenject;

namespace Game.Views.Cards
{
    /// <summary>
    /// View class for card.
    /// </summary>
    public class CardView : MonoBehaviour
    {
        private const float SelectedOffset = 0.5f;
        private const float TweenDuration = 0.5f;

        [SerializeField] private SpriteRenderer _cardTypeRenderer;
        [SerializeField] private SpriteRenderer _cardNoRenderer;
        private Card _card;
        private CardViewData _cardViewData;
        private PositionAnglePair _target;
        private Tween _tween;
        private bool _selected;

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

        public PositionAnglePair Target
        {
            get => _target;
            set
            {
                _target = value;
                RefreshTween();
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value) return;
                _selected = value;
                RefreshTween();
            }
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

        private void RefreshTween()
        {
            _tween.Kill();
            _tween = DOTween.Sequence()
                .Append(transform.DOMove(Target.Position + (Selected ? SelectedOffset * Vector3.up : Vector3.zero),
                    TweenDuration)).Join(transform.DORotate(new Vector3(0f, 0f, Target.Angle), TweenDuration));
        }

        public class Pool : MonoMemoryPool<CardView> { }
    }
}
