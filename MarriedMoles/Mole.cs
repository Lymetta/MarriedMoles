using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Mole: IActor
    {
        static readonly int MAX_RAGE = 30; // static readonly are always written all caps

        static readonly Random RANDOM = new Random();
        protected int MAX_SEXINESS = 10;

        protected bool alive = true;
        protected Point location = new Point(-1, -1); // if all negative - actor is not in field

        protected Field field; // nuorodo i lauka, kurioje padetyje triusis yra
        protected int rageLevel;
        public int sexyLevel;

        public bool IsAlive => alive;

        public Point Location => location;



        public Mole(Field field, Point location, bool randomAge = false)  // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                                          //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            // if born during game - start full
            SetLocation(location);
            this.sexyLevel = RANDOM.Next(MAX_SEXINESS) + 1;





        }

        protected virtual void SetLocation(Point newLocation) // tells the field where we are
        {
            if (location.X > -1) // if we exist in the field
            {
                field.Clear(location); // tells field to clear us from current location 
            }
            location = newLocation;
            field.Place(this);
        }

        public virtual void SetDead()
        {
            alive = false;
            if (location.X > -1)
            {
                field.Clear(location);
                field = null; // for garbage collector, so it know the rabbit will not be used anymore and can be fully removed with all links
                location = new Point(-1, -1);
            }
        }



        protected virtual void IncreaseRage()
        {
            rageLevel++;


        }




        public virtual void DropBomb(List<IActor> newBomb)
        {
            var freeSpaces = field.MOLEGetFREEAdjacentLocations(location);
            if (freeSpaces.Count > 0)
            {
                var bomb = new Bomb(field, freeSpaces[0], 0);
                newBomb.Add(bomb);
                rageLevel = 0;

            }


        }




        public virtual void Act(List<IActor> newActors)
        {
            IncreaseRage();

            if (rageLevel >= MAX_RAGE)
            {
                DropBomb(newActors);
            }

            var freeLocation = field.MOLEGetFREEAdjacentLocations(location); // might return empty list (no free spaces)
            if (freeLocation.Count > 0)
            {
                SetLocation(freeLocation[0]);
            }



        }
    }
}
