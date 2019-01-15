using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Othello.Data
{
    public enum Player { White = 0, Black = 1 };

    [Serializable]
    public class PlayerData : INotifyPropertyChanged
    {
        private int numberOfPawns = 0;
        private int secondsElapsed = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlayerData()
        {
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

        public int SecondsElapsed
        {
            get { return this.secondsElapsed; }
            set
            {
                secondsElapsed = value;
                RaisePropertyChanged("SecondsElapsed");
            }
        }

        public void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
