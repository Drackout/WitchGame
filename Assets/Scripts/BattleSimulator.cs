using System.Collections.Generic;
using UnityEngine;

public class BattleSimulator : MonoBehaviour
{
    private void Start()
    {
        IList<Card> cards = new List<Card>
        {
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Sword, Element.Fire, 3)
        };

        IPlayer player = new AIPlayer();

        Witch witch = new Witch("Witch", 20, cards, 5, 4, player);

        IList<Creature> creatures = new List<Creature>
        {
            new Dummy("Dummy", 10, Element.None)
        };

        ILogger logger = new UnityLogger();

        Battle battle = new Battle(witch, creatures, logger);
        battle.Run();
    }
}
