using System.Text;
using Game.Models.Cards;
using Game.Models.Data;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private void Start()
    {
        var cardBatch = new CardBatch
        {
            new Card(CardNo.Ace, CardType.Spades),
            new Card(CardNo.Two, CardType.Spades),
            new Card(CardNo.Three, CardType.Spades),
            new Card(CardNo.Four, CardType.Spades),
            new Card(CardNo.Three, CardType.Diamonds),
            new Card(CardNo.Four, CardType.Diamonds),
            new Card(CardNo.Five, CardType.Diamonds),
            new Card(CardNo.Ace, CardType.Diamonds),
            new Card(CardNo.Ace, CardType.Hearts),
            new Card(CardNo.Four, CardType.Hearts),
            new Card(CardNo.Four, CardType.Clubs)
        };

        var grouping = GroupingAlgorithms.GetSmartGroups(cardBatch);
        var builder = new StringBuilder();
        for (int i = 0; i < grouping.Groups.Count; i++)
        {
            builder.AppendLine("Group " + (i + 1) + ":");
            var group = grouping.Groups[i];
            foreach (var card in group)
            {
                builder.AppendLine(card.ToString());
            }
            builder.AppendLine();
        }
        builder.AppendLine("Ungrouped Cards:");
        foreach (var card in grouping.Ungrouped)
        {
            builder.AppendLine(card.ToString());
        }
        Debug.Log(builder.ToString());
    }
}
