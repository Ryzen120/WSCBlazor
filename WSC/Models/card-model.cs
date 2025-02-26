// WSC/Models/card-model.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WSC.Models
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }

        public string Name { get; set; }

        public byte[] ImageData { get; set; }

        public string Expansion { get; set; }

        public string Rarity { get; set; }

        public string Color { get; set; }

        public string CardType { get; set; }

        public int Level { get; set; }

        public int Power { get; set; }

        public string Soul { get; set; }

        public int Cost { get; set; }

        public string Trigger { get; set; }

        public string Traits { get; set; }

        public string Effect { get; set; }

        public string Flavor { get; set; }

        public string Illustrator { get; set; }

        public string Side { get; set; }
    }
}