using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DeadOfWinterWPF.Controller;
using DeadOfWinterWPF.Exceptions;
using DeadOfWinterWPF.Models;

namespace DeadOfWinterWPF.Classes
{
    public class Location
    {
        public string Name { get; }
        public int Number { get; }
        public int SurvivorSpots { get; set; }
        public OutsideArea OutsideArea { get; set; }
        public Canvas Canvas { get; }

        public Image ZombieImage;
        private Image _barricadeImage;
        public readonly Deck Deck;

        public Location(string name, int number,
            int survivorSpots, int outsideSpots, Canvas canvas)
        {
            Name = name;
            Number = number;
            SurvivorSpots = survivorSpots;
            OutsideArea = new OutsideArea
            {
                NumberOfBarricades = 0,
                OutsideSpots = outsideSpots,
                Zombies = 0
            };
            Canvas = canvas;

            ZombieImage = ImageLoader.InitializeMedia(@"Zombie.png");

            _barricadeImage = ImageLoader.InitializeMedia(@"Barricade.png");

            Deck = new Deck();
        }

        public Card SearchCard()
        {
            return Deck.DrawCard();
        }

        public void BuildBarricade()
        {
            OutsideArea.BuildBarricade();
        }


        public void SpawnZombies()
        {
            List<Survivor> survivors =
                StateController.Instance.Players[0].Survivors.FindAll(s => s.CurrentLocation == this);
            foreach (var s in survivors)
            {
                this.AddZombie(ZombieImage);
            }
        }
    }
    public class Colony : Location
    {
        public new OutsideArea[] OutsideArea { get; private set; } 
        public int AmountOfTrash { get; private set; }
        public Colony(string name, int number,
            int survivorSpots, int outsideSpots, Canvas canvas) : base(name, number,
        survivorSpots, outsideSpots, canvas)
        {
            AmountOfTrash = 6;
        }

//        public override void BuildBarricade()
//        {
//            var loop = true;
//            var i = 0;
//            do
//            {
//                if (OutsideArea[i].OutsideSpots > 0)
//                {
//                    OutsideArea[i].BuildBarricade();
//                    loop = false;
//                }
//
//                i++;
//            } while (loop && i < 6);
//        }
        public void Clean()
        {
            if (AmountOfTrash > 3)
            {
                AmountOfTrash -= 3;
            }
            else if (AmountOfTrash > 0)
            {
                AmountOfTrash = 0;
            }
        }
    }

    
}

