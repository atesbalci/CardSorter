using Game.Models.Cards;
using UnityEngine;

namespace Game.Models.Data
{
    public static class GameRules
    {
        /// <summary>
        /// Determines the value of a card.
        /// </summary>
        public static int GetValue(this CardNo cardNo)
        {
            return Mathf.Clamp((int) cardNo, 1, 10);
        }
    }
}
