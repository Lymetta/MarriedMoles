using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal abstract class Animal : IActor
    {
        protected abstract int BREEDING_AGE { get; }
        protected abstract int MAX_AGE { get; }
        protected abstract double BREEDING_PROBABILITY { get; }
        protected abstract int MAX_LITTER_SIZE { get; }


        protected static readonly Random RANDOM = new Random();
        protected bool alive = true;
        protected Point location = new Point(-1, -1); // if all negative - actor is not in field
        protected int age;
        protected Field field; // nuorodo i lauka, kurioje padetyje triusis yra



        public bool IsAlive => alive;

        public Point Location => location;

        public abstract void Act(List<IActor> newActors);



        protected void SetLocation(Point newLocation) // tells the field where we are
        {
            if (location.X > -1) // if we exist in the field
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
        protected void IncreaseAge()
        {
            age++;
            if (age >= MAX_AGE)
            {
                SetDead();
            }
        }

        
    }
}
