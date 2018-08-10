using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DeadOfWinterWPF.Controller;
using DeadOfWinterWPF.Exceptions;

namespace DeadOfWinterWPF.Classes
{
    public class Survivor
    {
        public string Name { get; }
        public int Health { get;  set; }
        public int AttackAbility { get; }
        public int SearchAbility { get; }
        public Location CurrentLocation { get; private set; }
        public Image Picture { get; }
        public bool CanMove { get; private set; }

        public Survivor(int attackAbility, int searchAbility,
            int playerLocation, string name, string filepath)
        {

            Health = 2;
            AttackAbility = attackAbility;
            SearchAbility = searchAbility;
            CurrentLocation = StateController.Instance.Locations.Find(l => l.Number == playerLocation);
            Name = name;
            Picture = ImageLoader.InitializeMedia(filepath);
            CanMove = true;
        }

        public void Move(Location location)
        {
            if (!CanMove) throw new InvalidActionException();
            CurrentLocation.RemoveImage(Picture);
            CurrentLocation = location;
            StateController.Instance.RollExposureDice(this);
            if (Health > 0)
            {
                CurrentLocation.AddSurvivor(Picture);
            }
            else
            {
                CurrentLocation = StateController.Instance.Locations.Find(l => l.Name == "Graveyard");
                CurrentLocation.AddSurvivor(Picture);
            }
            CanMove = false;
        }

        public void Attack(ActionDice die)
        {
            var canAttack = die.Value >= AttackAbility;
            if (canAttack && CurrentLocation.OutsideArea.Zombies > 0)
            {
                die.Use();
                CurrentLocation.RemoveImage(CurrentLocation.ZombieImage);
                StateController.Instance.RollExposureDice(this);
                if (Health <= 0)
                {
                    CanMove = true;
                    Move(StateController.Instance.Locations.Find(l => l.Name == "Graveyard"));
                }
            }
            else
            {
                throw new InvalidActionException();
            }
        }

        public Card Search(ActionDice die)
        {
            var canSearch = die.Value >= AttackAbility;
            Card card = null;
            if (canSearch)
            {
                card = CurrentLocation.SearchCard();
                die.Use();
            }
            else
            {
                throw new InvalidActionException();
            }

            return card;
        }

        public void BuildBarricade(Location location)
        {
            CurrentLocation.BuildBarricade();
        }

        public void CleanTrash(ActionDice dice)
        {
            if (dice.IsUsed == false)
            {
                var colony = (Colony)StateController.Instance.Locations.Find(l => l.GetType() == typeof(Colony));
                colony.Clean();
                dice.Use();
            }
            else
            {
                throw new InvalidActionException();
            }
        }
    }
}