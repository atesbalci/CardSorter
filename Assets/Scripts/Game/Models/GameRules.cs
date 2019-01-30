using Game.Models.Cards;
using UnityEngine;

namespace Game.Models
{
    public static class GameRules
    {
        /// <summary>
        /// Cards to be drawn in the beginning.
        /// </summary>
        public const int CardsToDraw = 11;

        /// <summary>
        /// Determines the value of a card.
        /// </summary>
        public static int GetValue(this CardNo cardNo)
        {
            return Mathf.Clamp((int) cardNo, 1, 10);
        }
    }
}
