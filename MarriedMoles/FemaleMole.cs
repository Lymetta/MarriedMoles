using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class FemaleMole : Mole
    {
        static readonly int BREEDING_AGE = 30; // static readonly are always written all caps
        static readonly int MAX_AGE = 200;
        static readonly double BREEDING_PROBABILITY = 0.5;
        static readonly int MAX_LITTER_SIZE = 6;
        static readonly int MAX_RAGE = 50;
        static readonly int MAX_SAD = 60;
        static readonly Random RANDOM = new Random();

        public MaleMole Spouse;


        public bool isMarried;


        protected int age;
        protected int sadness;



        public FemaleMole(Field field, Point location, bool randomAge = false) : base(field, location) // make 2 constructors ( last one is true/false) // if rabbit is from beginning of game - randomage is given,
                                                                                                       //if born during game - no randomAge, start from 0.
        {
            this.field = field;
            // if born during game - start full
            SetLocation(location);
            this.isMarried = false;

            if (randomAge)
            {
                age = RANDOM.Next(MAX_AGE);

                sadness = RANDOM.Next(MAX_SAD);
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


        protected override void IncreaseRage()
        {
            if (isMarried)
            {
                rageLevel++;
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

        public override void DropBomb(List<IActor> newBomb)
        {
            var freeSpaces = field.MOLEGetFREEAdjacentLocations(location);
            if (freeSpaces.Count > 0)
            {
                var bomb = new Bomb(field, freeSpaces[0], 0);
                newBomb.Add(bomb);
                rageLevel = 0;

            }


        }
        private void GiveBirth(List<IActor> newMoles)
        {
            if (age >= BREEDING_AGE && RANDOM.NextDouble() < BREEDING_PROBABILITY) // nextdouble returns a number from 0 to 1
            {
                var locList = field.AdjacentLocations(location);
                foreach (var loc in locList)
                {
                    
                        var actor = field.GetActorAt(loc);
                        var maleMole = actor as MaleMole;
                        if (maleMole != null && maleMole.IsAlive && !maleMole.isMarried)
                        {
                            var newMoleCount = RANDOM.Next(MAX_LITTER_SIZE) + 1;
                            var freeSpaces = field.MOLEGetFREEAdjacentLocations(location);///////////// FOR MOLES MAKES NEW FREE LOC AREA
                            isMarried = true;
                            Spouse = maleMole;
                            maleMole.isMarried = true;
                            maleMole.Spouse = this;

                            while (freeSpaces.Count > 0 && newMoleCount > 0)
                            {
                                var babySex = RANDOM.NextDouble();

                                if (babySex < 0.4)
                                {
                                    var babyMole = new FemaleMole(field, freeSpaces[0], false);
                                    freeSpaces.RemoveAt(0);
                                    newMoleCount--;
                                    newMoles.Add(babyMole);
                                }
                                else
                                {
                                    var babyMole = new MaleMole(field, freeSpaces[0], false);
                                    freeSpaces.RemoveAt(0);
                                    newMoleCount--;
                                    newMoles.Add(babyMole);
                                }

                            }

                        }
                    
                }


            }
        }


        public void DeathByLoneliness()
        {
            if (!isMarried)
            {
                sadness++;
                if (sadness >= MAX_SAD)
                {
                    SetDead();
                }
            }



        }



        public override void Act(List<IActor> newActors)
        {
            IncreaseAge();
            IncreaseRage();


            if (alive)
            {
                GiveBirth(newActors);


                if (rageLevel >= MAX_RAGE)
                {
                    DropBomb(newActors);

                }
                else
                {
                    var freeLocation = field.MOLEGetFREEAdjacentLocations(location); // might return empty list (no free spaces)
                    if (freeLocation.Count > 0)
                    {
                        SetLocation(freeLocation[0]);

                    }
                    DeathByLoneliness();

                }

            }
        }
    }
}
