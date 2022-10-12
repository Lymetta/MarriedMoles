using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Bomb: IActor
    {
        static readonly int EXPLOSION_TIME = 10;

        private bool alive = true;
        private Point location = new Point(-1, -1); // if all negative - actor is not in field

        private Field field; // nuorodo i lauka, kurioje padetyje triusis yra
        private int explosionTimer;

        public bool IsAlive => alive;

        public Point Location => location;


        public Bomb(Field field, Point location, int timer) // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                            //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            SetLocation(location);
            this.alive = true;
            this.explosionTimer = timer;





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

        private void IncreaseTimer()
        {
            explosionTimer++;
            if (explosionTimer >= EXPLOSION_TIME)
            {
                Explosion();
                SetDead();
            }
        }

        public void Explosion()
        {
            var list = field.DeathByMoleLocations(location);
            foreach (var loc in list)
            {
                var actor = field.GetActorAt(loc);
                /*var rabbit = actor as Rabbit;
                var fox = actor as Fox;
                var mole = actor as Mole;*/
                var bomb = actor as Bomb;


                if (actor != null && actor.IsAlive && bomb == null)
                {
                    actor.SetDead();

                }
                /*else if (fox != null && fox.IsAlive)
                {
                    fox.SetDead();
                } else if (mole != null && mole.IsAlive)
                {
                    mole.SetDead();
                } */

            }

        }



        public void Act(List<IActor> newActors)
        {

            if (alive)
            {
                IncreaseTimer();

            }
        }
    }
}
