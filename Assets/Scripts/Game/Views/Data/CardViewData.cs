using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Models.Cards;
using UnityEngine;

namespace Game.Views.Data
{
    /// <summary>
    /// A data class for CardView to access required common data among card views.
    /// </summary>
    public class CardViewData
    {
        private readonly ReadOnlyCollection<Sprite> _cardTypeSprites;
        private readonly ReadOnlyCollection<Sprite> _cardNoSprites;

        public CardViewData(IList<Sprite> cardTypeSprites, IList<Sprite> cardNoSprites)
        {
            _cardTypeSprites = new ReadOnlyCollection<Sprite>(cardTypeSprites);
            _cardNoSprites = new ReadOnlyCollection<Sprite>(cardNoSprites);
        }

        public Sprite GetCardTypeSprite(CardType cardType)
        {
            return _cardTypeSprites[(int) cardType - 1];
        }

        public Sprite GetCardNoSprite(CardNo cardNo)
        {
            return _cardNoSprites[(int) cardNo - 1];
        }
    }
}
