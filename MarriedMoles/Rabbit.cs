using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Rabbit: Animal
    {
        protected override int BREEDING_AGE => 5; // static readonly are always written all caps
        protected override int MAX_AGE => 40;
        protected override double BREEDING_PROBABILITY => 0.10;
        protected override int MAX_LITTER_SIZE => 4;
        


        

        public Rabbit(Field field, Point location, bool randomAge = false) // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                                           //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            SetLocation(location);
            if (randomAge)
            {
                age = RANDOM.Next(MAX_AGE);
            }


        }

        
        private void GiveBirth(List<IActor> newRabbits)
        {
            if (age >= BREEDING_AGE && RANDOM.NextDouble() < BREEDING_PROBABILITY) // nextdouble returns a number from 0 to 1
            {
                var newRabbitsCount = RANDOM.Next(MAX_LITTER_SIZE) + 1; // at least one baby
                var freeSpaces = field.GetFREEAdjacentLocations(location);
                while (freeSpaces.Count > 0 && newRabbitsCount > 0)
                {
                    var rabbit = new Rabbit(field, freeSpaces[0]);
                    freeSpaces.RemoveAt(0);
                    newRabbitsCount--;
                    newRabbits.Add(rabbit);
                }
            }
        }

        public override void Act(List<IActor> newActors)
        {
            IncreaseAge();
            if (alive)
            {
                GiveBirth(newActors);
                var freeLocation = field.GetFREEAdjacentLocations(location); // might return empty list (no free spaces)
                if (freeLocation.Count > 0)
                {
                    SetLocation(freeLocation[0]);
                }
                else
                {
                    SetDead(); // if rabbit cant move to another location, he dies
                }
            }
        }
    }
}
