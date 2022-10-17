using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal class Field
    {
        int width, height;
        IActor[,] field;
        private Random random = new Random();

        public int Width // you can get the width/height, but cant change it
        {
            get => width;
        }
        public int Height => height;

        public Field(int width, int height)
        {
            this.width = width;
            this.height = height;
            field = new IActor[width, height]; // initialize field with the coordinates that you get (a 2d array of actors)
        }

        public void Clear() // clear the field
        {
            //field = new IActor[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    field[i, j] = null;
                }
            }
        }


        public void Clear(Point position) // remove a specific actor
        {
            field[position.X, position.Y] = null;
        }


        public void Place(IActor actor) // adds actor - additional method (assuming actor knows his position)
        {
            field[actor.Location.X, actor.Location.Y] = actor;
        }
        public void Place(IActor actor, Point position) // adds actor
        {
            field[position.X, position.Y] = actor;
        }
        public IActor GetActorAt(Point position) // returns null ( if loc is empty) or actor, gets via position
        {
            return field[position.X, position.Y];
        }

        public IActor GetActorAt(int x, int y) // gets actor via coordinates
        {
            return field[x, y];
        }

        public List<Point> AdjacentLocations(Point position)
        {

            var list = new List<Point>();
            //x, y

            for (int xOffSet = -1; xOffSet <= 1; xOffSet++)
            {
                var x = position.X + xOffSet;
                if (x >= 0 && x < width)// checks if within bounds of field
                {
                    for (int yOffSet = -1; yOffSet <= 1; yOffSet++)
                    {
                        var y = position.Y + yOffSet;
                        if (y >= 0 && y < height)
                        {
                            if (xOffSet != 0 || yOffSet != 0) // doesnt add to list if original position 
                            {
                                list.Add(new Point(x, y));
                            }
                        }
                    }

                }


            }

            list = RandomizedList(list); // mixes up the positions in the list
            return list;
        }
        public List<Point> MOLEAdjacentLocations(Point position)
        {

            var list = new List<Point>();
            //x, y

            for (int xOffSet = -2; xOffSet <= 2; xOffSet++)
            {
                var x = position.X + xOffSet;
                if (x >= 0 && x < width)// checks if within bounds of field
                {
                    for (int yOffSet = -2; yOffSet <= 2; yOffSet++)
                    {
                        var y = position.Y + yOffSet;
                        if (y >= 0 && y < height)
                        {
                            if (xOffSet != 0 || yOffSet != 0) // doesnt add to list if original position 
                            {
                                list.Add(new Point(x, y));
                            }
                        }
                    }

                }


            }

            list = RandomizedList(list); // mixes up the positions in the list
            return list;
        }
        public List<Point> GetFREEAdjacentLocations(Point position)
        {
            var list = AdjacentLocations(position);
            var free = new List<Point>();
            foreach (var pos in list)
            {
                if (GetActorAt(pos) == null || (GetActorAt(pos) is Mole)) // checks if there's an actor there 
                {
                    free.Add(pos);
                }
            }

            return free;

        }

        public List<Point> MOLEGetFREEAdjacentLocations(Point position)
        {
            var list = MOLEAdjacentLocations(position);
            var free = new List<Point>();
            foreach (var pos in list)
            {
                if ((GetActorAt(pos) == null) || (GetActorAt(pos) is Fox) || (GetActorAt(pos) is Rabbit)) // checks if there's an actor there 
                {
                    free.Add(pos);
                }
            }

            return free;

        }

        private List<Point> RandomizedList(List<Point> list) // fishhook method // returns random list
        {

            for(int i = list.Count - 1; i > 0; i--)
            {
                var j = random.Next(i);
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }

        public List<Point> DeathByMoleLocations(Point position, int range = 5)
        {
            var list = new List<Point>();
            for (int xOffSet = -range; xOffSet <= range; xOffSet++)
            {
                var x = position.X + xOffSet;
                if (x >= 0 && x < width)// checks if within bounds of field
                {
                    for (int yOffSet = -range; yOffSet <= range; yOffSet++)
                    {
                        var y = position.Y + yOffSet;
                        if (y >= 0 && y < height)
                        {
                            if (xOffSet * xOffSet + yOffSet * yOffSet <= range * range) /////
                                list.Add(new Point(x, y));

                        }
                    }

                }

            }
            return list;
        }
    }
}
