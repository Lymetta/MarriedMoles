using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MarriedMoles
{
    internal class Fox : Animal
    {
        protected override double BREEDING_PROBABILITY => 0.35;
        protected override int BREEDING_AGE => 10;
        protected override int MAX_AGE => 150;
        protected override int MAX_LITTER_SIZE => 5;

        static readonly int RABBIT_FOOD_VALUE = 7; // 


        private int foodLevel; // how much the fox has eaten, if reaches 0 - fox dies of hunger



        public Fox(Field field, Point location, bool randomAge = false) // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                                        //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            foodLevel = RABBIT_FOOD_VALUE; // if born during game - start full
            SetLocation(location);
            if (randomAge)
            {
                age = RANDOM.Next(MAX_AGE);
                foodLevel = RANDOM.Next(RABBIT_FOOD_VALUE); // random hunger level at start of game
            }


        }



        protected override IActor NewChild(Field field, Point location)
        {
            return new Fox(field, location);
        }



        private void IncreaseHunger()
        {
            foodLevel--;
            if (foodLevel <= 0)
            {
                SetDead();
            }

        }




        Point FindFood()
        {
            var list = field.AdjacentLocations(location);
            foreach (var loc in list)
            {
                var actor = field.GetActorAt(loc);
                var rabbit = actor as Rabbit;
                if (rabbit != null && rabbit.IsAlive)
                {
                    rabbit.SetDead();
                    foodLevel = RABBIT_FOOD_VALUE;
                    return loc;
                }
            }

            return new Point(-1, -1); // return if there is no food around
        }

        public override void Act(List<IActor> newActors)
        {
            IncreaseAge();
            IncreaseHunger();
            if (alive)
            {
                GiveBirth(newActors);
                var huntPoint = FindFood();
                if (huntPoint.X > -1) // successfull hunt
                {
                    SetLocation(huntPoint);
                }
                else
                {
                    var freeLocation = field.GetFREEAdjacentLocations(location); // might return empty list (no free spaces)
                    if (freeLocation.Count > 0)
                    {
                        SetLocation(freeLocation[0]);
                    }
                    else
                    {
                        SetDead(); // if fox cant move to another location, he dies
                    }
                }
            }
        }
    }
}
