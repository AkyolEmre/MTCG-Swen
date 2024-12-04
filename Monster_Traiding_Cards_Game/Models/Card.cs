using System;

namespace Monster_Trading_Cards_Game.Models
{
    public enum ElementType { Water, Fire, Normal }

    public abstract class Card
    {
        public string Name { get; }
        public int Damage { get; private set; }
        public ElementType CardElementType { get; }

        protected Card(string name)
        {
            Name = name;
            CardElementType = DetermineElementType(name);
            Damage = AssignDamage();
        }

        private ElementType DetermineElementType(string name) => name switch
        {
            "Dragons" or "FireElves" or "Amaterasu" or "Raijin" or "Tetsu" => ElementType.Fire,
            "Kraken" or "DogMike" or "Susanoo" or "Bankai" or "Wizzard" => ElementType.Water,
            _ => ElementType.Normal
        };

        private int AssignDamage() => CardElementType switch
        {
            ElementType.Normal => 50,
            ElementType.Fire => 70,
            ElementType.Water => 70,
            _ => 50
        };

        public double CalculateDamage(Card opponent)
        {
            double effectiveness = (CardElementType, opponent.CardElementType) switch
            {
                (ElementType.Water, ElementType.Fire) => 2.0,
                (ElementType.Fire, ElementType.Normal) => 2.0,
                (ElementType.Normal, ElementType.Water) => 2.0,
                (ElementType.Fire, ElementType.Water) => 0.5,
                (ElementType.Normal, ElementType.Fire) => 0.5,
                (ElementType.Water, ElementType.Normal) => 0.5,
                _ => 1.0
            };
            return Damage * effectiveness;
        }

        public abstract void PlayCard();
    }
}
