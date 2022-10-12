using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Simulator
    {
        static readonly double FOX_CREATION_PROBABILITY = 0.02;
        static readonly double RABBIT_CREATION_PROBABILITY = 0.08;
        static readonly double MALE_MOLE_CREATION_PROBABILITY = 0.01;
        static readonly double FEMALE_MOLE_CREATION_PROBABILITY = 0.009;


        private Field field;
        private List<IActor> actors;


        public int Step { get; private set; }

        public event Action<Field> StepDone;

        public Simulator(int width, int height)
        {

            field = new Field(width, height);
            actors = new List<IActor>();
            Step = 0;
            Populate();
        }

        public void Run(int n)
        {
            for (int i = 0; i < n; i++)
            {
                RunOneStep();
            }
        }


        public void RunOneStep()
        {
            Step++;
            var newActors = new List<IActor>();
            foreach (var actor in actors)
            {
                actor.Act(newActors);
            }
            actors.AddRange(newActors);

            //reserve.AddRange(actors.Where(a => !a.IsAlive));
            actors.RemoveAll(a => !a.IsAlive);

            //for (int i = 0; i < actors.Count; i++)
            //{
            //  if (!actors[i].IsAlive)
            //  {
            //    actors.RemoveAt(i);
            //    i--;
            //  }
            //}

            StepDone?.Invoke(field);
        }

        public void Reset()
        {
            Step = 0;
            actors.Clear();
            Populate();
        }

        private void Populate()
        {
            var rand = new Random();
            field.Clear();

            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    if (rand.NextDouble() < FEMALE_MOLE_CREATION_PROBABILITY)
                    {
                        actors.Add(new FemaleMole(field, new Point(x, y), true));
                    }
                    else if (rand.NextDouble() < MALE_MOLE_CREATION_PROBABILITY)
                    {
                        actors.Add(new MaleMole(field, new Point(x, y), true));
                    }
                    else if (rand.NextDouble() < FOX_CREATION_PROBABILITY)
                    {
                        actors.Add(new Fox(field, new Point(x, y), true));

                    }
                    else if (rand.NextDouble() < RABBIT_CREATION_PROBABILITY)
                    {
                        var rabbit = new Rabbit(field, new Point(x, y), true); // uses a bit more memory, but easier to read
                        actors.Add(rabbit);
                    }
                }
            }
        }
    }
}
