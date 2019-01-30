using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Game.Views.Data
{
    public class CardViewData
    {
        public ReadOnlyCollection<Sprite> CardTypeSprites { get; }
        public ReadOnlyCollection<Sprite> CardNoSprites { get; }

        public CardViewData(IList<Sprite> cardTypeSprites, IList<Sprite> cardNoSprites)
        {
            CardTypeSprites = new ReadOnlyCollection<Sprite>(cardTypeSprites);
            CardNoSprites = new ReadOnlyCollection<Sprite>(cardNoSprites);
        }
    }
}
