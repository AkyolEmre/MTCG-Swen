using System;

namespace Monster_Trading_Cards_Game.Models
{
    public enum ElementType
    {
        Water,
        Fire,
        Normal
    }

    public abstract class Card
    {
        public string Name { get; set; }
        public int BaseDamage { get; private set; } // Grundschaden basierend auf Typ
        public ElementType CardElementType { get; private set; }

        public Card(string name)
        {
            Name = name;
            CardElementType = AssignElementType(name); // Elementtyp basierend auf dem Namen zuweisen
            AssignBaseDamage(); // Basisschaden basierend auf dem Typ zuweisen
        }

        // Methode zur Zuweisung des richtigen Elementtyps basierend auf dem Kartennamen
        private ElementType AssignElementType(string name)
        {
            switch (name)
            {
                case "Phoenix":
                case "InfernoMage":
                case "BlazingTitan":
                case "LavaFiend":
                case "PyroGolem":
                    return ElementType.Fire; // Fire Typ

                case "AquaSerpent":
                case "StormCaller":
                case "TsunamiDragon":
                case "WaveRider":
                case "SeaElemental":
                    return ElementType.Water; // Water Typ

                case "StoneGiant":
                case "IronClad":
                case "ShadowBeast":
                case "ForestStalker":
                case "MountainGuardian":
                    return ElementType.Normal; // Normal Typ

                default:
                    return ElementType.Normal; // Standardwert
            }
        }

        // Methode zur Zuweisung des Basisschadens basierend auf dem Typ
        private void AssignBaseDamage()
        {
            switch (CardElementType)
            {
                case ElementType.Normal:
                    BaseDamage = 50; // Normalkarten-Schaden
                    break;
                case ElementType.Fire:
                    BaseDamage = 60; // Feuerkarten-Schaden
                    break;
                case ElementType.Water:
                    BaseDamage = 55; // Wasserkarten-Schaden
                    break;
                default:
                    BaseDamage = 50; // Standardwert
                    break;
            }
        }

        // Die berechnete Schaden-Eigenschaft
        public int Damage
        {
            get
            {
                return CalculateDamage(); // Berechnet den Schaden, wenn er angefordert wird
            }
        }

        // Berechnung des Schadens basierend auf dem Typ und Gegner
        private int CalculateDamage()
        {
            // Beispiel, wie der Schaden multipliziert wird
            double typeModifier = GetTypeModifier(CardElementType);
            return (int)(BaseDamage * typeModifier);
        }

        // Regel für Schadensmodifikatoren basierend auf Typeninteraktion
        private double GetTypeModifier(ElementType opponentType)
        {
            if (CardElementType == ElementType.Water && opponentType == ElementType.Fire)
                return 1.8; // Wasser besiegt Feuer
            if (CardElementType == ElementType.Fire && opponentType == ElementType.Normal)
                return 1.6; // Feuer besiegt Normal
            if (CardElementType == ElementType.Normal && opponentType == ElementType.Water)
                return 1.7; // Normal besiegt Wasser

            if (CardElementType == ElementType.Fire && opponentType == ElementType.Water)
                return 0.7; // Feuer verliert gegen Wasser
            if (CardElementType == ElementType.Normal && opponentType == ElementType.Fire)
                return 0.8; // Normal verliert gegen Feuer
            if (CardElementType == ElementType.Water && opponentType == ElementType.Normal)
                return 0.6; // Wasser verliert gegen Normal

            return 1.0; // Gleiche Typen oder keine spezielle Regel: Normaler Schaden
        }

        public abstract void PlayCard();
    }
}
