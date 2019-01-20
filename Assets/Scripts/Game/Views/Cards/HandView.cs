using System.Collections.Generic;
using System.Linq;
using Game.Models.Cards;
using Helpers.Vectors;
using UnityEngine;

namespace Game.Views.Cards
{
    [RequireComponent(typeof(RadialPlacer))]
    public class HandView : MonoBehaviour
    {
        private const float PerIndexZOffset = -0.2f;
        private static readonly  Vector3 CardSpawnPos = new Vector3(0f, 10f, 0f);

        [SerializeField] private CardView _cardPrefab;
        private CardBatch _hand;
        private Dictionary<Card, CardView> _cardViews;
        private RadialPlacer _radialPlacer;
        private Camera _camera;

        public void Bind(CardBatch hand)
        {
            _hand = hand;
            _cardViews = new Dictionary<Card, CardView>();
            _radialPlacer = GetComponent<RadialPlacer>();
            _camera = Camera.main;
            _hand.OnAdd += OnAdd;
            _hand.OnChange += OnChange;
            _hand.OnRemove += OnRemove;
        }

        private void Update()
        {
            var mousePos = (Vector2) _camera.ScreenToWorldPoint(Input.mousePosition);
            var index = -1;

            if (_radialPlacer.GetDistanceFromPerimeter(mousePos) < 1f)
            {
                index = _radialPlacer.GetIndex(mousePos, _hand.Count);
                if(Input.GetMouseButton(0))
                {
                    var selectedCardView = _cardViews.FirstOrDefault(kvp => kvp.Value.Selected);
                    if (selectedCardView.Value != null)
                    {
                        var selectedIndex = _hand.IndexOf(selectedCardView.Key);
                        if (index >= 0 && index < _hand.Count && selectedIndex != index)
                        {
                            _hand.SwitchIndex(selectedIndex, index);
                        }
                    }
                }
            }

            for (int i = 0; i < _hand.Count; i++)
            {
                _cardViews[_hand[i]].Selected = i == index;
            }
        }

        private void OnDestroy()
        {
            _hand.OnAdd -= OnAdd;
            _hand.OnChange -= OnChange;
            _hand.OnRemove -= OnRemove;
        }

        private void OnAdd(Card card)
        {
            var cardView = Instantiate(_cardPrefab, transform, false);
            cardView.transform.position = CardSpawnPos;
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
                cardView.TargetPosition = new Vector3(pos.Position.x, pos.Position.y, i * PerIndexZOffset);
                cardView.TargetRotation = Quaternion.AngleAxis(-pos.Angle, Vector3.forward);
            }
        }

        private void OnRemove(Card card)
        {
            var cardView = _cardViews[card];
            _cardViews.Remove(card);
            Destroy(cardView.gameObject);
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
