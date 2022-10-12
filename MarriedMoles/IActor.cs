using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarriedMoles
{
    internal interface IActor
    {
        void Act(List<IActor> newActors); // returns rabbits and foxes that are born during that step

        bool IsAlive { get; }

        Point Location { get; }

        void SetDead();
    }
}
