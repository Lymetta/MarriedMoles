using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class MaleMole :Mole
    { // static readonly are always written all caps
        static readonly int MAX_AGE = 1000;
        static readonly int MAX_HORNINESS = 5;


        static readonly Random RANDOM = new Random();
        public bool isMarried;
        private int hornyLevel;

        public FemaleMole Spouse;

        protected int age;
        public bool cheater;




        public MaleMole(Field field, Point location, bool randomAge = false) : base(field, location) // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                                                                     //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            // if born during game - start full
            SetLocation(location);
            this.isMarried = false;
            Spouse = null;
            this.cheater = false;
            this.sexyLevel = RANDOM.Next(MAX_SEXINESS) + 1;
            if (randomAge)
            {
                age = RANDOM.Next(MAX_AGE);
                // random hunger level at start of game
            }


        }

        protected override void SetLocation(Point newLocation) // tells the field where we are
        {
            if (location.X > -1) // if we exist in the field
            {
                field.Clear(location); // tells field to clear us from current location 
            }
            location = newLocation;
            field.Place(this);
        }


        public override void SetDead()
        {
            alive = false;
            if (location.X > -1)
            {
                field.Clear(location);
                field = null; // for garbage collector, so it know the rabbit will not be used anymore and can be fully removed with all links
                location = new Point(-1, -1);
            }
        }



        private void IncreaseAge()
        {
            age++;
            if (age >= MAX_AGE)
            {
                SetDead();
            }
        }

        private void IncreaseSexy()
        {
            if (isMarried || cheater)
            {
                if (sexyLevel >= MAX_SEXINESS)
                {
                    sexyLevel = 1;
                }
                else
                {
                    sexyLevel++;
                }
            }
        }


        private void IncreaseHorny()
        {
            if (isMarried)
            {
                hornyLevel++;
            }
        }

        private void ResetMarriageStartCheat()
        {
            if (hornyLevel >= MAX_HORNINESS)
            {
                isMarried = false;
                hornyLevel = 0;
                cheater = true;
            }
        }

        

        public override void Act(List<IActor> newActors)
        {
            IncreaseAge();

            if (alive)
            {
                IncreaseHorny();
                IncreaseSexy();
                ResetMarriageStartCheat();
                
                var freeLocation = field.MOLEGetFREEAdjacentLocations(location); // might return empty list (no free spaces)
                if (freeLocation.Count > 0)
                {
                    SetLocation(freeLocation[0]);
                }



            }
        }
    }
}
