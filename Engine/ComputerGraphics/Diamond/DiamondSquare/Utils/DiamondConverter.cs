using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace terraformerIsland.DiamondAlghorithm.Utils
{
    class DiamondConverter
    {
        public static int RoundUp(int multiple, float toRound)
        {
            return (int) ( (multiple - toRound % multiple) + toRound );
        }

        public static int RoundDown(int multiple, float toRound)
        {
            return (int) (toRound - toRound % multiple);
        }

        public static int RoundNear(int multiple, float toRound)
        {
            float up = RoundUp(multiple, toRound);
            float down = RoundDown(multiple, toRound);

            float diffUp = Math.Abs(toRound - up);
            float diffDown = Math.Abs(toRound - down);

            if (diffUp < diffDown)
                return (int)up;
            else
                return (int)down;

        }
    }
}
