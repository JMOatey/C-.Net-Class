using System;
using DeadOfWinterWPF.Exceptions;

namespace DeadOfWinterWPF.Classes
{
    public class ActionDice
    {
        public int Number { get; set; }
        public int Value { get; set; }
        protected virtual int MaxDiceRange { get; set; } = 7;
        protected static Random DiceRoll { get; set; }
        public bool IsUsed { get; private set; }

        private static int _number = -2;

        public ActionDice()
        {
            Number = ++_number;
            IsUsed = false;
            if (DiceRoll == null)
            {
                DiceRoll = new Random();
            }
        }

        public void Roll()
        {
            Value = DiceRoll.Next(1, MaxDiceRange);
            IsUsed = false;
        }

        public void Use()
        {
            if (IsUsed == false)
            {
                IsUsed = true;
            }
            else
            {
                throw new InvalidActionException();
            }

        }
    }

    public class ExposureDice : ActionDice
    {
        protected override int MaxDiceRange { get; set; } = 13;
        private ExposureValue _exposureName;

        public new ExposureValue Roll()
        {
            base.Roll();
            SetExposureName();
            return _exposureName;
        }

        private void SetExposureName()
        {
            switch (Value)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    _exposureName = ExposureValue.NoExposure;
                    break;
                case 7:
                case 8:
                case 9:
                    _exposureName = ExposureValue.Wounded;
                    break;
                case 10:
                case 11:
                    _exposureName = ExposureValue.FrostBitten;
                    break;
                case 12:
                    _exposureName = ExposureValue.Bitten;
                    break;

            }
        }

    }
    public enum ExposureValue
    {
        NoExposure,
        Wounded,
        FrostBitten,
        Bitten
    }
}