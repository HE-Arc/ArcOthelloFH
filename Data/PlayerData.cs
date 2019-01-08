using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    enum Player { White = 0, Black = 1 };

    class PlayerData
    {
        private int numberOfPawns = 0;
        private int secondsElapsed = 0;
        private Player color;

        public PlayerData(Player color)
        {
            this.color = color;
        }

        public int NumberOfPawns
        {
            get { return numberOfPawns; }
            set { numberOfPawns = value; }
        }

        public int SecondsElapsed
        {
            get { return secondsElapsed ; }
            set { secondsElapsed = value; } 
        }

        public Player Color
        {
            get { return color ;}
        }
    }
}
