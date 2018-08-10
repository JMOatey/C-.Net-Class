using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DeadOfWinterWPF.Classes
{
    public class Deck : Collection<Card>
    {
        public int NumberOfCardsLeft { get; private set; }

        private Random _random;

        public Deck()
        {
            _random = new Random();
            for (var i = 0; i < 4; i++)
            {
                var item = (Item) i;
                for (var j = 0; j < 5; j++)
                {
                    var card = new Card(item);
                    this.Add(card);
                }
            };
            NumberOfCardsLeft = this.Count;
        }

        public Deck(int handSize)
        {
            _random = new Random();
            for (var i = 0; i < handSize; i++)
            {
                var card = new Card((Item) _random.Next(4));
                this.Add(card);
            }
        }

        private int GetRandomIndex()
        {
            return _random.Next(this.Count);
        }

        public Card DrawCard()
        {
            var randomIndex = GetRandomIndex();
            var card = this[randomIndex];

            this.Remove(card);
            NumberOfCardsLeft--;
            return card;
        }
    }

    public class Card
    {
        public Item Value { get; private set; }
        public string Name { get; private set; }

        public Card(Item value)
        {
            Value = value;
            switch (value)
            {
                case Item.Fuel:
                    Name = "Fuel";
                    break;
                case Item.Food:
                    Name = "Food";
                    break;
                case Item.Tools:
                    Name = "Tools";
                    break;
                case Item.Survivor:
                    Name = "Survivor";
                    break;
            }
        }
    }
}
