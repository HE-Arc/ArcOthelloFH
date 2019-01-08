using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour GameControlsUI.xaml
    /// </summary>
    public partial class GameControlsUI : UserControl
    {
        private event EventHandler eventEndTurnPressed;

        public GameControlsUI()
        {
            InitializeComponent();
        }

        public EventHandler EventEndTurnPressed
        {
            get { return this.eventEndTurnPressed; }
            set { this.eventEndTurnPressed = value; }
        }

        private void lbl_end_turn_Click(object sender, RoutedEventArgs e)
        {
            EventHandler handler = eventEndTurnPressed;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
