using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Rabbit: IActor
    {
        static readonly int BREEDING_AGE = 5; // static readonly are always written all caps
        static readonly int MAX_AGE = 40;
        static readonly double BREEDING_PROBABILITY = 0.10;
        static readonly int MAX_LITTER_SIZE = 4;
        static readonly Random RANDOM = new Random();


        private bool alive = true;
        private Point location = new Point(-1, -1); // if all negative - actor is not in field
        private int age;
        private Field field; // nuorodo i lauka, kurioje padetyje triusis yra

        public bool IsAlive => alive;

        public Point Location => location;


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

        private void SetLocation(Point newLocation) // tells the field where we are
        {
            if (location.X > -1) // if we exist in the field !!!!!!!!!!!!!!!!!!!!!
            {
                field.Clear(location); // tells field to clear us from current location 
            }
            location = newLocation;
            field.Place(this);
        }


        public void SetDead()
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

        public void Act(List<IActor> newActors)
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
