using System.Collections.Generic;
using Game.Models.Cards;
using Helpers.Vectors;
using UnityEngine;
using Zenject;

namespace Game.Views.Cards
{
    /// <summary>
    /// Observes the assigned hand card batch object and also has the ability to reorganize it.
    /// </summary>
    [RequireComponent(typeof(RadialPlacer))]
    public class HandView : MonoBehaviour
    {
        private const float PerIndexZOffset = -0.2f;
        private static readonly  Vector3 CardSpawnPos = new Vector3(0f, 10f, 0f);

        private CardBatch _hand;
        private Dictionary<Card, CardView> _cardViews;
        private RadialPlacer _radialPlacer;
        private Camera _camera;
        private Card _selectedCard;
        private CardView.Pool _cardPool;

        [Inject]
        public void Initialize(CardView.Pool cardPool, CardBatch hand)
        {
            _cardPool = cardPool;
            _hand = hand;
            _cardViews = new Dictionary<Card, CardView>();
            _radialPlacer = GetComponent<RadialPlacer>();
            _camera = Camera.main;
            _hand.OnAdd += OnAdd;
            _hand.OnChange += OnChange;
            _hand.OnRemove += OnRemove;
            _radialPlacer.OnAdapt += OnChange;
        }

        // Only performs calculations while the mouse/touch is held down
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePos = (Vector2) _camera.ScreenToWorldPoint(Input.mousePosition);
                if (_radialPlacer.GetDistanceFromPerimeter(mousePos) < 1f)
                {
                    var index = _radialPlacer.GetIndex(mousePos, _hand.Count);
                    if (Input.GetMouseButtonDown(0))
                    {
                        SelectedCard = _hand[index];
                    }
                    else if (SelectedCard != null)
                    {
                        var selectedIndex = _hand.IndexOf(SelectedCard);
                        if (selectedIndex != index)
                        {
                            _hand.SwitchIndex(selectedIndex, index);
                        }
                    }
                }
            }
            else
            {
                SelectedCard = null;
            }
        }

        private void OnDestroy()
        {
            _hand.OnAdd -= OnAdd;
            _hand.OnChange -= OnChange;
            _hand.OnRemove -= OnRemove;
            if (_radialPlacer != null) _radialPlacer.OnAdapt -= OnChange;
        }

        private Card SelectedCard
        {
            get => _selectedCard;
            set
            {
                if(_selectedCard == value) return;
                if (_selectedCard != null)
                {
                    _cardViews[SelectedCard].Selected = false;
                }
                _selectedCard = value;
                if (value != null)
                {
                    _cardViews[SelectedCard].Selected = true;
                }
            }
        }

        private void OnAdd(Card card)
        {
            var cardView = _cardPool.Spawn();
            var cardTrans = cardView.transform;
            cardTrans.SetParent(transform);
            cardTrans.position = CardSpawnPos;
            _cardViews[card] = cardView;
            cardView.Bind(card);
        }

        private void OnChange()
        {
            for (var i = 0; i < _hand.Count; i++)
            {
                var card = _hand[i];
                var cardView = _cardViews[card];
                var pos = _radialPlacer.GetSlotPosAndAngle(i, _hand.Count);
                pos.Position.z = i * PerIndexZOffset;
                cardView.Target = pos;
            }
        }

        private void OnRemove(Card card)
        {
            var cardView = _cardViews[card];
            _cardViews.Remove(card);
            _cardPool.Despawn(cardView);
        }

        public void OneTwoThreeSort()
        {
            _hand.SortByGrouping(GroupType.OneTwoThree);
        }

        public void SevenSevenSevenSort()
        {
            _hand.SortByGrouping(GroupType.SevenSevenSeven);
        }

        public void SmartSort()
        {
            _hand.SortBySmartGrouping();
        }
    }
}
