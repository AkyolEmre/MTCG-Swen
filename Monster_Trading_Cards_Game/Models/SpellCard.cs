using System;

namespace Monster_Trading_Cards_Game.Models
{
    public class SpellCard : Card
    {
        public SpellCard(string name) : base(name)
        {
        }

        public override void PlayCard()
        {
            Console.WriteLine($"Spell card '{Name}' is cast! It has {CardElementType} element.");
        }
    }
}
