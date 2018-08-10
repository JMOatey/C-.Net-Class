using System.Collections.Generic;
using System.Runtime.InteropServices;
using DeadOfWinterWPF.Exceptions;

namespace DeadOfWinterWPF.Classes
{
    public class Player
    {
        public string PlayerName { get; private set; }

        public List<Survivor> Survivors { get; private set; }
        public Survivor SelectedSurvivor { get; set; }
        public List<ActionDice> Dice { get; private set; }
        public Deck Hand { get; private set; }
        public ActionDice SelectedDice { get; set; }
        public Card SelectedCard { get; set; }

        public Player()
        {
            Survivors = new List<Survivor>();
            Dice = new List<ActionDice>();
            Hand = new Deck(5);

            PlayerName = "John";
            Survivor survivor1 = new Survivor(4, 1, 0, "Sora", @"Sora_KHIII.png");
            Survivor survivor2 = new Survivor(1, 6, 0, "Riku", @"Riku.png");
            survivor1.CurrentLocation.AddSurvivor(survivor1.Picture);
            survivor2.CurrentLocation.AddSurvivor(survivor2.Picture);

            Survivors.Add(survivor1);
            Survivors.Add(survivor2);

            Dice.Add(new ActionDice());
            Dice.Add(new ActionDice());
        }

        public void Search(Survivor survivor, ActionDice die)
        {
            Card card = survivor.Search(die);
            if (card != null)
            {
                Hand.Add(card);
            }
            else
            {
                throw new InvalidActionException();
            }
        }

        public void Attack(Survivor survivor, ActionDice die)
        {
            survivor.Attack(die);
        }
    }
}