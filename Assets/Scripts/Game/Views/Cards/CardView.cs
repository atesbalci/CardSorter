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
        [SerializeField] private GameObject _idleCardBack;
        [SerializeField] private GameObject _selectedCardBack;
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

        /// <summary>
        /// Binds to the card and refreshes it's view.
        /// </summary>
        public void Bind(Card card)
        {
            _card = card;
            RefreshCardVisuals();
            Selected = false;
            RefreshSelected();
        }

        /// <summary>
        /// Current movement target of the card view.
        /// </summary>
        public PositionAnglePair Target
        {
            get => _target;
            set
            {
                _target = value;
                RefreshTween();
            }
        }

        /// <summary>
        /// Determines if the card is selected.
        /// </summary>
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value) return;
                _selected = value;
                RefreshTween();
                RefreshSelected();
            }
        }

        private void RefreshCardVisuals()
        {
            _cardTypeRenderer.sprite = _cardViewData.GetCardTypeSprite(_card.CardType);
            _cardNoRenderer.sprite = _cardViewData.GetCardNoSprite(_card.CardNo);
            var color = (_card.CardType == CardType.Hearts || _card.CardType == CardType.Diamonds)
                ? Color.red
                : Color.black;
            _cardNoRenderer.color = color;
            _cardTypeRenderer.color = color;
        }

        /// <summary>
        /// Runs the movement tween.
        /// </summary>
        private void RefreshTween()
        {
            _tween.Kill();
            _tween = DOTween.Sequence()
                .Append(transform.DOMove(Target.Position + (Selected ? SelectedOffset * Vector3.up : Vector3.zero),
                    TweenDuration)).Join(transform.DORotate(new Vector3(0f, 0f, Target.Angle), TweenDuration));
        }

        private void RefreshSelected()
        {
            _idleCardBack.SetActive(!Selected);
            _selectedCardBack.SetActive(Selected);
        }

        /// <summary>
        /// Pool class for card game objects.
        /// </summary>
        public class Pool : MonoMemoryPool<CardView> { }
    }
}
