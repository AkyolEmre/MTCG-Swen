using System;

namespace Monster_Trading_Cards_Game.Models
{
    public class NormalCard : Card
    {
        public NormalCard(string name) : base(name)
        {
        }

        public override void PlayCard()
        {
            Console.WriteLine($"Normal card '{Name}' is played! It belongs to the {CardElementType} element and deals {BaseDamage} base damage.");
        }
    }
}
