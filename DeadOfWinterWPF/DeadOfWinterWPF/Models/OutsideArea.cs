using DeadOfWinterWPF.Exceptions;

namespace DeadOfWinterWPF.Models
{
    public class OutsideArea
    {
        public int OutsideSpots { get; set; }
        public int NumberOfBarricades { get; set; }
        public int Zombies { get; set; }

        public OutsideArea()
        {
            OutsideSpots = 0;
            NumberOfBarricades = 0;
        }

        public void BuildBarricade()
        {
            if (OutsideSpots > 0)
            {
                NumberOfBarricades++;
                OutsideSpots--;
            }
            else
            {
                throw new InvalidActionException();
            }
        }
    }
}