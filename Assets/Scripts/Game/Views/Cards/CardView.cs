using Game.Models.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.Cards
{
    public class CardView : MonoBehaviour
    {
        private const float LerpMultiplier = 5f;
        private const float SelectedOffset = 0.5f;

        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }
        public bool Selected { get; set; }

        [SerializeField] private TextMeshPro _cardTypeText;
        [SerializeField] private TextMeshPro _cardNoText;
        private Card _card;

        public void Bind(Card card)
        {
            _card = card;
            _cardNoText.text = _card.CardNo.ToString();
            _cardTypeText.text = _card.CardType.ToString();
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
