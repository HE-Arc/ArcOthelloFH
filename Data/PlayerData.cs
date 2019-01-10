using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Othello.Data
{
    enum Player { White = 0, Black = 1 };

    class PlayerData
    {
        private int numberOfPawns = 0;
        private int secondsElapsed = 0;
        private Player color;
        private Stopwatch stopwatch;

        public PlayerData(Player color)
        {
            this.color = color;
            this.stopwatch = new Stopwatch();
        }

        public void StartTimer()
        {
            this.stopwatch.Start();
        }

        public void StopTimer()
        {
            this.stopwatch.Stop();
        }

        public int NumberOfPawns
        {
            get { return numberOfPawns; }
            set { numberOfPawns = value; }
        }

        public int SecondsElapsed
        {
            get { return stopwatch.Elapsed.Seconds; }
            set { secondsElapsed = value; } 
        }

        public Player Color
        {
            get { return color ;}
        }
    }
}
