using System;

namespace Monster_Trading_Cards_Game.Models
{
    public class MonsterCard : Card
    {
        public MonsterCard(string name) : base(name) { }

        public override void PlayCard() => Console.WriteLine($"Monster card '{Name}' with element {CardElementType} is played.");
    }
}
