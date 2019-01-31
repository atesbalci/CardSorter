namespace Game.Models.Cards
{
    /// <summary>
    /// A basic card class
    /// </summary>
    public class Card
    {
        public CardNo CardNo { get; }
        public CardType CardType { get; }

        public Card(CardNo cardNo, CardType cardType)
        {
            CardNo = cardNo;
            CardType = cardType;
        }

        public int Value => CardNo.GetValue();

        public override string ToString()
        {
            return CardType + " - " + CardNo;
        }

        public bool Equals(Card card)
        {
            return card.CardNo == CardNo && card.CardType == CardType;
        }
    }

    public enum CardNo
    {
        None = 0,
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    public enum CardType
    {
        None = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
        Spades = 4
    }
}
