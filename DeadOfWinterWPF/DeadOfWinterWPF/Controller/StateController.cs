using System.Collections.Generic;
using System.Windows.Controls;
using DeadOfWinterWPF.Classes;

namespace DeadOfWinterWPF.Controller
{
    public sealed class StateController
    {
        public List<Location> Locations { get; private set; }
        public Location SelectedLocation { get; set; }
        public static StateController Instance { get; } = new StateController();
        public List<Player> Players { get; private set; }
        public Player SelectedPlayer { get; set; }
        public ExposureDice ExposureDice { get; }

        public StateController()
        {
            Locations = new List<Location>();
            Players = new List<Player>();
            ExposureDice = new ExposureDice();
        }

        public void RollExposureDice(Survivor survivor)
        {
            var result = ExposureDice.Roll();
            switch (result)
            {
                case ExposureValue.FrostBitten:
                case ExposureValue.Wounded:
                    survivor.Health--;
                    break;
                case ExposureValue.Bitten:
                    survivor.Health = 0;
                    break;
                default:
                    survivor.Health = survivor.Health;
                    break;
            }
        }

        public void BeginTurn()
        {
            RollActionDice();
        }

        public void EndTurn()
        {
            foreach (Location l in Locations)
            {
                l.SpawnZombies();
            }
        }

        private void RollActionDice()
        {
            foreach (Player p in Players)
            {
                foreach (ActionDice d in p.Dice)
                {
                    d.Roll();
                }
            }
        }
    }
}