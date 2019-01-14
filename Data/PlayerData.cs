using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Othello.Data
{
    enum Player { White = 0, Black = 1 };

    class PlayerData : INotifyPropertyChanged
    {
        private int numberOfPawns = 0;
        private int secondsElapsed = 0;
        private Player color;
        private Stopwatch stopwatch;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public void ClearScore()
        {
            this.numberOfPawns = 0;
        }

        public int NumberOfPawns
        {
            get { return numberOfPawns; }
            set
            {
                numberOfPawns = value;
                RaisePropertyChanged("NumberOfPawns");
            }
        }

        public void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
